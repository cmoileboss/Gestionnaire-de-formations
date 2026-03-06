from sqlalchemy.orm import Session
from models.Evaluation import Evaluation
from datetime import datetime


class EvaluationRepository:
    """
    Repository SQLAlchemy pour la gestion des évaluations.
    Fournit des méthodes CRUD et de filtrage sur les évaluations.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, start_date: datetime = None, end_date: datetime = None, 
               place: str = None, module_id: int = None) -> Evaluation:
        """
        Crée une nouvelle évaluation.
        :param start_date: Date de début
        :param end_date: Date de fin
        :param place: Lieu de l'évaluation
        :param module_id: Identifiant du module associé
        :return: L'évaluation créée
        """
        evaluation = Evaluation(
            start_date=start_date,
            end_date=end_date,
            place=place,
            module_id=module_id
        )
        self.db.add(evaluation)
        self.db.commit()
        self.db.refresh(evaluation)
        return evaluation

    def get_by_id(self, evaluation_id: int) -> Evaluation:
        """
        Récupère une évaluation par son identifiant unique.
        :param evaluation_id: Identifiant évaluation
        :return: L'évaluation ou None
        """
        return self.db.query(Evaluation).filter(
            Evaluation.evaluation_id == evaluation_id
        ).first()

    def get_by_module(self, module_id: int) -> list[Evaluation]:
        """
        Récupère toutes les évaluations d'un module donné.
        :param module_id: Identifiant module
        :return: Liste d'évaluations
        """
        return self.db.query(Evaluation).filter(
            Evaluation.module_id == module_id
        ).all()

    def get_all(self) -> list[Evaluation]:
        """
        Récupère toutes les évaluations avec pagination.
        :param skip: Décalage de départ
        :param limit: Nombre maximum de résultats
        :return: Liste d'évaluations
        """
        return self.db.query(Evaluation).all()

    def update(self, evaluation_id: int, start_date: datetime = None, 
               end_date: datetime = None, place: str = None, module_id: int = None) -> Evaluation:
        """
        Met à jour les informations d'une évaluation existante.
        :param evaluation_id: Identifiant évaluation
        :param start_date: Nouvelle date de début (optionnel)
        :param end_date: Nouvelle date de fin (optionnel)
        :param place: Nouveau lieu (optionnel)
        :param module_id: Nouvel identifiant module (optionnel)
        :return: L'évaluation mise à jour ou None
        """
        evaluation = self.get_by_id(evaluation_id)
        if evaluation:
            if start_date is not None:
                evaluation.start_date = start_date
            if end_date is not None:
                evaluation.end_date = end_date
            if place is not None:
                evaluation.place = place
            if module_id is not None:
                evaluation.module_id = module_id
            self.db.commit()
            self.db.refresh(evaluation)
        return evaluation

    def delete(self, evaluation_id: int) -> bool:
        """
        Supprime une évaluation de la base.
        :param evaluation_id: Identifiant évaluation
        :return: True si supprimé, False sinon
        """
        evaluation = self.get_by_id(evaluation_id)
        if evaluation:
            self.db.delete(evaluation)
            self.db.commit()
            return True
        return False
