from fastapi import Depends, HTTPException
from fastapi.security import APIKeyCookie
from models.User import User
from repositories.UserRepository import UserRepository
from sqlalchemy.orm import Session
from database_connection import get_db
from jose import jwt, JWTError
from datetime import datetime, timedelta, timezone
from dotenv import load_dotenv
import os


load_dotenv()
SECRET_KEY = os.getenv("SECRET_KEY")
ALGORITHM = os.getenv("ALGORITHM")
ACCESS_TOKEN_EXPIRE_MINUTES = int(os.getenv("ACCESS_TOKEN_EXPIRE_MINUTES", 30))

cookie_scheme = APIKeyCookie(name="access_token", auto_error=False)

class SecurityService:

    @staticmethod
    def create_access_token(email: str) -> str:
        """Create a JWT access token for the given user email.
        Args:
            email (str): The email of the user.
        Returns:
            token (str): The encoded JWT token.
        """
        expire = datetime.now(tz=timezone.utc) + timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)

        payload = {
            "sub": str(email),
            "iat": datetime.now(tz=timezone.utc),
            "exp": expire
        }

        return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)

    @staticmethod
    def verify_access_token(token: str) -> str:
        try:
            payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])

            email = payload.get("sub")
            if email is None:
                raise ValueError("Token invalide (sub manquant)")

            return email

        except JWTError:
            raise ValueError("Token invalide ou expiré")
        except Exception as e:
            raise ValueError(f"Erreur lors de la vérification du token : {str(e)}")
        
    async def get_current_user(db: Session = Depends(get_db), access_token: str = Depends(cookie_scheme)):
        if access_token is None:
            raise HTTPException(status_code=401, detail="Non authentifié")

        try:
            user_repository = UserRepository(db)
            email = SecurityService.verify_access_token(access_token)
            user = user_repository.get_by_email(email)
            if user is None:
                raise HTTPException(status_code=401, detail="Utilisateur non trouvé")
            return user
        except ValueError as e:
            raise HTTPException(status_code=401, detail=str(e))