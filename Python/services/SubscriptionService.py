from datetime import datetime
from sqlalchemy.orm import Session

from repositories.SubscriptionRepository import SubscriptionRepository
from models.Subscription import Subscription
from exceptions import NotFoundError


class SubscriptionService:
    def __init__(self, db: Session):
        self.subscription_repository = SubscriptionRepository(db)


    def get_all_subscriptions(self) -> list[Subscription]:
        """Retourne la liste de tous les abonnements."""
        return self.subscription_repository.get_all()

    def get_subscriptions_by_user(self, user_id: int) -> list[Subscription]:
        """Retourne tous les abonnements d'un utilisateur."""
        return self.subscription_repository.get_by_user(user_id)

    def get_subscriptions_by_session(self, session_id: int) -> list[Subscription]:
        """Retourne tous les abonnements pour une session."""
        return self.subscription_repository.get_by_session(session_id)

    def get_subscription(self, user_id: int, session_id: int) -> Subscription:
        """Retourne un abonnement spécifique, ou lève NotFoundError."""
        subscription = self.subscription_repository.get(user_id, session_id)
        if subscription is None:
            raise NotFoundError("Abonnement", f"{user_id}/{session_id}")
        return subscription

    def create_subscription(self, user_id: int, session_id: int, subscription_date: datetime) -> Subscription:
        """Crée un nouvel abonnement."""
        return self.subscription_repository.create(user_id, session_id, subscription_date)

    def delete_subscription(self, user_id: int, session_id: int) -> None:
        """Supprime un abonnement. Lève NotFoundError si introuvable."""
        success = self.subscription_repository.delete(user_id, session_id)
        if not success:
            raise NotFoundError("Abonnement", f"{user_id}/{session_id}")
