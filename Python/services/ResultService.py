from sqlalchemy.orm import Session

from repositories.ResultRepository import ResultRepository
from models.Module import Module
from exceptions import NotFoundError


class ResultService:
    def __init__(self, db: Session):
        self.result_repository = ResultRepository(db)

    def get_all_results(self):
        """Retourne la liste de tous les résultats d'évaluation."""
        return self.result_repository.get_all()
    
    def create_result(self, user_id: int, evaluation_id: int, score: float = None, success: bool = None, date=None):
        """Crée un nouveau résultat d'évaluation."""
        return self.result_repository.create(user_id, evaluation_id, score, success, date)
    
    def delete_result(self, user_id: int, evaluation_id: int) -> None:
        """Supprime un résultat d'évaluation. Lève NotFoundError si introuvable."""
        success = self.result_repository.delete(user_id, evaluation_id)
        if not success:
            raise NotFoundError("Résultat", f"user_id={user_id}, evaluation_id={evaluation_id}")
    
    def get_result(self, user_id: int, evaluation_id: int):
        """Retourne un résultat d'évaluation par sa clé composite, ou lève NotFoundError."""
        result = self.result_repository.get_by_composite_key(user_id, evaluation_id)
        if result is None:
            raise NotFoundError("Résultat", f"user_id={user_id}, evaluation_id={evaluation_id}")
        return result

    def get_results_by_user(self, user_id: int):
        """Retourne tous les résultats d'évaluation pour un utilisateur donné."""
        return self.result_repository.get_by_user(user_id)

    def get_results_by_evaluation(self, evaluation_id: int):
        """Retourne tous les résultats d'évaluation pour une évaluation donnée."""
        return self.result_repository.get_by_evaluation(evaluation_id)
    
    def update_result(self, user_id: int, evaluation_id: int, score: float = None, success: bool = None, date=None):
        """Met à jour un résultat d'évaluation. Lève NotFoundError si introuvable."""
        result = self.result_repository.update(user_id, evaluation_id, score, success, date)
        if result is None:
            raise NotFoundError("Résultat", f"user_id={user_id}, evaluation_id={evaluation_id}")
        return result