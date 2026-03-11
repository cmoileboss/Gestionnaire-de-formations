import bcrypt
from datetime import datetime
from sqlalchemy.orm import Session

from repositories.UserRepository import UserRepository
from repositories.SubscriptionRepository import SubscriptionRepository
from repositories.SessionRepository import SessionRepository
from repositories.EvaluationRepository import EvaluationRepository
from models.User import User
from models.Session import Session as SessionModel
from models.Evaluation import Evaluation
from exceptions import NotFoundError, DuplicateError, UnauthorizedError


class UserService:
    def __init__(self, db: Session):
        self.user_repository = UserRepository(db)
        self.subscription_repository = SubscriptionRepository(db)
        self.session_repository = SessionRepository(db)
        self.evaluation_repository = EvaluationRepository(db)

    def login(self, email: str, password: str) -> User:
        """Vérifie les identifiants et retourne un User si valides, sinon lève UnauthorizedError."""
        user = self.user_repository.get_by_email(email)
        if user is None:
            raise UnauthorizedError("Email ou mot de passe invalide")
        if not bcrypt.checkpw(password.encode("utf-8"), user.password_hash.encode("utf-8")):
            raise UnauthorizedError("Email ou mot de passe invalide")
        return user
    
    def get_all_users(self) -> list[User]:
        """Retourne la liste de tous les utilisateurs."""
        return self.user_repository.get_all()

    def get_user(self, user_id: int) -> User:
        """Retourne un utilisateur par son identifiant, ou lève NotFoundError."""
        user = self.user_repository.get_by_id(user_id)
        if user is None:
            raise NotFoundError("Utilisateur", user_id)
        return user

    def create_user(self, email: str, password: str, address: str = None) -> User:
        """Crée un utilisateur en hachant son mot de passe avec bcrypt."""
        hashed = bcrypt.hashpw(password.encode("utf-8"), bcrypt.gensalt()).decode("utf-8")
        return self.user_repository.create(email, hashed, address)

    def update_user(self, user_id: int, email: str = None, password: str = None, address: str = None) -> User:
        """Met à jour un utilisateur. Hache le mot de passe si fourni. Lève NotFoundError si introuvable."""
        if password:
            password = bcrypt.hashpw(password.encode("utf-8"), bcrypt.gensalt()).decode("utf-8")
        updated = self.user_repository.update(user_id, email, password, address)
        if updated is None:
            raise NotFoundError("Utilisateur", user_id)
        return updated

    def delete_user(self, user_id: int) -> None:
        """Supprime un utilisateur. Lève NotFoundError si introuvable."""
        success = self.user_repository.delete(user_id)
        if not success:
            raise NotFoundError("Utilisateur", user_id)


    def get_user_sessions(self, user_id: int) -> list[SessionModel]:
        """Retourne la liste des sessions auxquelles l'utilisateur est inscrit."""
        # Vérifier que l'utilisateur existe
        user = self.user_repository.get_by_id(user_id)
        if user is None:
            raise NotFoundError("Utilisateur", user_id)
        
        subscriptions = self.subscription_repository.get_by_user(user_id)
        return [sub.session for sub in subscriptions]

    def subscribe_to_session(self, user_id: int, session_id: int) -> None:
        """Inscrit un utilisateur à une session. Lève NotFoundError ou DuplicateError."""
        # Vérifier que l'utilisateur existe
        user = self.user_repository.get_by_id(user_id)
        if user is None:
            raise NotFoundError("Utilisateur", user_id)
        
        # Vérifier que la session existe
        session = self.session_repository.get_by_id(session_id)
        if session is None:
            raise NotFoundError("Session", session_id)
        
        existing = self.subscription_repository.get(user_id, session_id)
        if existing:
            raise DuplicateError(f"L'utilisateur {user_id} est déjà inscrit à la session {session_id}")
        self.subscription_repository.create(user_id, session_id, datetime.utcnow())

    def unsubscribe_from_session(self, user_id: int, session_id: int) -> None:
        """Désinscrit un utilisateur d'une session. Lève NotFoundError si non inscrit."""
        success = self.subscription_repository.delete(user_id, session_id)
        if not success:
            raise NotFoundError("Abonnement", f"{user_id}/{session_id}")
