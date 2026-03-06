from sqlalchemy.orm import Session
from models.Result import Result
from datetime import datetime


class ResultRepository:
    """
    Repository SQLAlchemy pour la gestion des résultats d'évaluation.
    Fournit des méthodes CRUD et de filtrage sur les résultats.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, user_id: int, evaluation_id: int, score: float, success: bool, 
               date: datetime = None) -> Result:
        """
        Crée un nouveau résultat d'évaluation.
        :param user_id: Identifiant de l'utilisateur
        :param evaluation_id: Identifiant de l'évaluation
        :param score: Score obtenu
        :param success: True si réussi, False sinon
        :param date: Date du résultat (date actuelle par défaut)
        :return: Le résultat créé
        """
        result = Result(
            user_id=user_id,
            evaluation_id=evaluation_id,
            score=score,
            success=success,
            date=date or datetime.utcnow()
        )
        self.db.add(result)
        self.db.commit()
        self.db.refresh(result)
        return result

    def get_by_id(self, result_id: int) -> Result:
        """
        Récupère un résultat par son identifiant unique.
        :param result_id: Identifiant résultat
        :return: Le résultat ou None
        """
        return self.db.query(Result).filter(
            Result.result_id == result_id
        ).first()

    def get_all(self) -> list[Result]:
        """
        Récupère tous les résultats avec pagination.
        :param skip: Décalage de départ
        :param limit: Nombre maximum de résultats
        :return: Liste de résultats
        """
        return self.db.query(Result).all()

    def update(self, result_id: int, score: float = None, success: bool = None, 
               date: datetime = None) -> Result:
        """
        Met à jour un résultat existant.
        :param result_id: Identifiant résultat
        :param score: Nouveau score (optionnel)
        :param success: Nouveau statut de réussite (optionnel)
        :param date: Nouvelle date (optionnel)
        :return: Le résultat mis à jour ou None
        """
        result = self.get_by_id(result_id)
        if result:
            if score is not None:
                result.score = score
            if success is not None:
                result.success = success
            if date is not None:
                result.date = date
            self.db.commit()
            self.db.refresh(result)
        return result

    def delete(self, result_id: int) -> bool:
        """
        Supprime un résultat de la base.
        :param result_id: Identifiant résultat
        :return: True si supprimé, False sinon
        """
        result = self.get_by_id(result_id)
        if result:
            self.db.delete(result)
            self.db.commit()
            return True
        return False
