from sqlalchemy.orm import Session
from models.AIRecommandation import AIRecommandation
from datetime import datetime


class AIRecommandationRepository:
    """
    Repository SQLAlchemy pour la gestion des recommandations IA.
    Fournit des méthodes CRUD et de recherche sur les recommandations IA.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, session_id: int, confidence: int, date: datetime = None) -> AIRecommandation:
        """
        Crée une nouvelle recommandation IA pour une session.
        :param session_id: Identifiant session
        :param confidence: Niveau de confiance (1-99)
        :param date: Date de la recommandation (date actuelle par défaut)
        :return: La recommandation créée
        """
        recommendation = AIRecommandation(
            session_id=session_id,
            confidence=confidence,
            date=date or datetime.utcnow()
        )
        self.db.add(recommendation)
        self.db.commit()
        self.db.refresh(recommendation)
        return recommendation

    def get_by_id(self, recommendation_id: int) -> AIRecommandation:
        """
        Récupère une recommandation IA par son identifiant unique.
        :param recommendation_id: Identifiant recommandation
        :return: La recommandation ou None
        """
        return self.db.query(AIRecommandation).filter(
            AIRecommandation.recommendation_id == recommendation_id
        ).first()

    def get_by_session(self, session_id: int) -> list[AIRecommandation]:
        """
        Récupère toutes les recommandations IA d'une session donnée.
        :param session_id: Identifiant session
        :return: Liste de recommandations
        """
        return self.db.query(AIRecommandation).filter(
            AIRecommandation.session_id == session_id
        ).all()

    def get_all(self) -> list[AIRecommandation]:
        """
        Récupère toutes les recommandations IA avec pagination.
        :param skip: Décalage de départ
        :param limit: Nombre maximum de résultats
        :return: Liste de recommandations
        """
        return self.db.query(AIRecommandation).all()

    def update(self, recommendation_id: int, confidence: int = None, date: datetime = None) -> AIRecommandation:
        """
        Met à jour une recommandation IA existante.
        :param recommendation_id: Identifiant recommandation
        :param confidence: Nouveau niveau de confiance (optionnel)
        :param date: Nouvelle date (optionnel)
        :return: La recommandation mise à jour ou None
        """
        recommendation = self.get_by_id(recommendation_id)
        if recommendation:
            if confidence is not None:
                recommendation.confidence = confidence
            if date is not None:
                recommendation.date = date
            self.db.commit()
            self.db.refresh(recommendation)
        return recommendation

    def delete(self, recommendation_id: int) -> bool:
        """
        Supprime une recommandation IA de la base.
        :param recommendation_id: Identifiant recommandation
        :return: True si supprimé, False sinon
        """
        recommendation = self.get_by_id(recommendation_id)
        if recommendation:
            self.db.delete(recommendation)
            self.db.commit()
            return True
        return False
