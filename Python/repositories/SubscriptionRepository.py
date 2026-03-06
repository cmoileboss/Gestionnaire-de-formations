from sqlalchemy.orm import Session
from models.Subscription import Subscription


class SubscriptionRepository:
    """
    Repository SQLAlchemy pour la gestion des inscriptions (abonnements) aux sessions.
    Fournit des méthodes CRUD et de recherche sur les abonnements.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, user_id: int, session_id: int, subscription_date: str) -> Subscription:
        """
        Crée une nouvelle inscription d'un utilisateur à une session.
        :param user_id: Identifiant utilisateur
        :param session_id: Identifiant session
        :param subscription_date: Date d'inscription
        :return: L'inscription créée
        """
        subscription = Subscription(
            user_id=user_id,
            session_id=session_id,
            subscription_date=subscription_date
        )
        self.db.add(subscription)
        self.db.commit()
        self.db.refresh(subscription)
        return subscription

    def get(self, user_id: int, session_id: int) -> Subscription:
        """
        Récupère une inscription spécifique par identifiants utilisateur et session.
        :param user_id: Identifiant utilisateur
        :param session_id: Identifiant session
        :return: L'inscription ou None
        """
        return self.db.query(Subscription).filter(
            Subscription.user_id == user_id,
            Subscription.session_id == session_id
        ).first()

    def get_by_user(self, user_id: int) -> list[Subscription]:
        """
        Récupère toutes les inscriptions d'un utilisateur donné.
        :param user_id: Identifiant utilisateur
        :return: Liste d'inscriptions
        """
        return self.db.query(Subscription).filter(Subscription.user_id == user_id).all()

    def get_by_session(self, session_id: int) -> list[Subscription]:
        """
        Récupère toutes les inscriptions pour une session donnée.
        :param session_id: Identifiant session
        :return: Liste d'inscriptions
        """
        return self.db.query(Subscription).filter(Subscription.session_id == session_id).all()

    def get_all(self) -> list[Subscription]:
        """
        Récupère toutes les inscriptions avec pagination.
        :param skip: Décalage de départ
        :param limit: Nombre maximum de résultats
        :return: Liste d'inscriptions
        """
        return self.db.query(Subscription).all()

    def update_date(self, user_id: int, session_id: int, subscription_date: str) -> Subscription:
        """
        Met à jour la date d'inscription d'un utilisateur à une session.
        :param user_id: Identifiant utilisateur
        :param session_id: Identifiant session
        :param subscription_date: Nouvelle date d'inscription
        :return: L'inscription mise à jour ou None
        """
        subscription = self.get(user_id, session_id)
        if subscription:
            subscription.subscription_date = subscription_date
            self.db.commit()
            self.db.refresh(subscription)
        return subscription

    def delete(self, user_id: int, session_id: int) -> bool:
        """
        Supprime une inscription d'un utilisateur à une session.
        :param user_id: Identifiant utilisateur
        :param session_id: Identifiant session
        :return: True si supprimé, False sinon
        """
        subscription = self.get(user_id, session_id)
        if subscription:
            self.db.delete(subscription)
            self.db.commit()
            return True
        return False