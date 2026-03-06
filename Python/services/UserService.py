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


class UserService:
    def __init__(self, db: Session):
        self.user_repository = UserRepository(db)
        self.subscription_repository = SubscriptionRepository(db)
        self.session_repository = SessionRepository(db)
        self.evaluation_repository = EvaluationRepository(db)

    def login(self, email: str, password: str) -> str:
        """Vérifie les identifiants et retourne un JWT si valides."""
        user = self.user_repository.get_by_email(email)
        if user is None:
            raise ValueError("Invalid email or password")
        if not bcrypt.checkpw(password.encode("utf-8"), user.password_hash.encode("utf-8")):
            raise ValueError("Invalid email or password")
        return user
    
    def get_all_users(self) -> list[User]:
        """Retourne la liste de tous les utilisateurs."""
        return self.user_repository.get_all()

    def get_user(self, user_id: int) -> User | None:
        """Retourne un utilisateur par son identifiant, ou None."""
        return self.user_repository.get_by_id(user_id)

    def create_user(self, email: str, password: str, address: str = None) -> User:
        """Crée un utilisateur en hachant son mot de passe avec bcrypt."""
        hashed = bcrypt.hashpw(password.encode("utf-8"), bcrypt.gensalt()).decode("utf-8")
        return self.user_repository.create(email, hashed, address)

    def update_user(self, user_id: int, email: str = None, password: str = None, address: str = None) -> User | None:
        """Met à jour un utilisateur. Hache le mot de passe si fourni."""
        if password:
            password = bcrypt.hashpw(password.encode("utf-8"), bcrypt.gensalt()).decode("utf-8")
        return self.user_repository.update(user_id, email, password, address)

    def delete_user(self, user_id: int) -> bool:
        """Supprime un utilisateur. Retourne True si supprimé."""
        return self.user_repository.delete(user_id)


    def get_user_sessions(self, user_id: int) -> list[SessionModel]:
        """Retourne la liste des sessions auxquelles l'utilisateur est inscrit."""
        subscriptions = self.subscription_repository.get_by_user(user_id)
        return [sub.session for sub in subscriptions]

    def subscribe_to_session(self, user_id: int, session_id: int) -> bool:
        """Inscrit un utilisateur à une session. Lève ValueError si déjà inscrit."""
        session = self.session_repository.get_by_id(session_id)
        if session is None:
            return None
        existing = self.subscription_repository.get(user_id, session_id)
        if existing:
            raise ValueError("User already subscribed to this session")
        self.subscription_repository.create(user_id, session_id, datetime.utcnow())
        return True

    def unsubscribe_from_session(self, user_id: int, session_id: int) -> bool:
        """Désinscrit un utilisateur d'une session. Retourne True si succès."""
        return self.subscription_repository.delete(user_id, session_id)


    def get_user_evaluations(self, user_id: int) -> list[Evaluation]:
        """Retourne les évaluations de l'utilisateur (non implémenté)."""
        return []

    def enroll_in_evaluation(self, user_id: int, evaluation_id: int) -> bool:
        """Inscrit un utilisateur à une évaluation (non implémenté)."""
        return False

    def unenroll_from_evaluation(self, user_id: int, evaluation_id: int) -> bool:
        """Désinscrit un utilisateur d'une évaluation (non implémenté)."""
        return False