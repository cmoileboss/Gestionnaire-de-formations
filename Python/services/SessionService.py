from datetime import datetime
from sqlalchemy.orm import Session as DbSession

from repositories.SessionRepository import SessionRepository
from repositories.SubscriptionRepository import SubscriptionRepository
from models.Session import Session as SessionModel
from models.User import User


class SessionService:
    def __init__(self, db: DbSession):
        self.session_repository = SessionRepository(db)
        self.subscription_repository = SubscriptionRepository(db)


    def get_all_sessions(self) -> list[SessionModel]:
        """Retourne la liste de toutes les sessions."""
        return self.session_repository.get_all()

    def get_session(self, session_id: int) -> SessionModel | None:
        """Retourne une session par son identifiant, ou None."""
        return self.session_repository.get_by_id(session_id)

    def create_session(self, formation_id: int, start_date: datetime, end_date: datetime, place: str) -> SessionModel:
        """Crée une nouvelle session de formation."""
        return self.session_repository.create(formation_id, start_date, end_date, place)

    def update_session(self, session_id: int, formation_id: int = None, start_date: datetime = None,
                       end_date: datetime = None, place: str = None) -> SessionModel | None:
        """Met à jour une session existante. Retourne None si introuvable."""
        return self.session_repository.update(session_id, formation_id, start_date, end_date, place)

    def delete_session(self, session_id: int) -> bool:
        """Supprime une session. Retourne True si supprimée."""
        return self.session_repository.delete(session_id)


    def get_session_users(self, session_id: int) -> list[User]:
        """Retourne tous les utilisateurs inscrits à une session."""
        subscriptions = self.subscription_repository.get_by_session(session_id)
        return [sub.user for sub in subscriptions]
