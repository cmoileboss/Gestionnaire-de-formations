from datetime import datetime
from sqlalchemy.orm import Session

from repositories.EvaluationRepository import EvaluationRepository
from models.Evaluation import Evaluation
from models.User import User
from exceptions import NotFoundError


class EvaluationService:
    def __init__(self, db: Session):
        self.evaluation_repository = EvaluationRepository(db)


    def get_all_evaluations(self) -> list[Evaluation]:
        """Retourne la liste de toutes les évaluations."""
        return self.evaluation_repository.get_all()

    def get_evaluation(self, evaluation_id: int) -> Evaluation:
        """Retourne une évaluation par son identifiant, ou lève NotFoundError."""
        evaluation = self.evaluation_repository.get_by_id(evaluation_id)
        if evaluation is None:
            raise NotFoundError("Évaluation", evaluation_id)
        return evaluation

    def create_evaluation(self, start_date: datetime, end_date: datetime,
                          place: str, module_id: int) -> Evaluation:
        """Crée une nouvelle évaluation."""
        return self.evaluation_repository.create(start_date, end_date, place, module_id)

    def update_evaluation(self, evaluation_id: int, start_date: datetime = None,
                          end_date: datetime = None, place: str = None,
                          module_id: int = None) -> Evaluation:
        """Met à jour une évaluation existante. Lève NotFoundError si introuvable."""
        updated = self.evaluation_repository.update(evaluation_id, start_date, end_date, place, module_id)
        if updated is None:
            raise NotFoundError("Évaluation", evaluation_id)
        return updated

    def delete_evaluation(self, evaluation_id: int) -> None:
        """Supprime une évaluation. Lève NotFoundError si introuvable."""
        success = self.evaluation_repository.delete(evaluation_id)
        if not success:
            raise NotFoundError("Évaluation", evaluation_id)


    def get_evaluation_users(self, evaluation_id: int) -> list[User]:
        """Retourne les utilisateurs inscrits à une évaluation (non implémenté)."""
        return []
