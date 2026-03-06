from sqlalchemy.orm import Session

from repositories.ResultRepository import ResultRepository
from models.Module import Module


class ResultService:
    def __init__(self, db: Session):
        self.result_repository = ResultRepository(db)

    def get_all_results(self):
        """Retourne la liste de tous les résultats d'évaluation."""
        return self.result_repository.get_all()
    
    def create_result(self, user_id: int, evaluation_id: int, score: float, success: bool, date=None):
        """Crée un nouveau résultat d'évaluation."""
        return self.result_repository.create(user_id, evaluation_id, score, success, date)
    
    def delete_result(self, result_id: int):
        """Supprime un résultat d'évaluation."""
        return self.result_repository.delete(result_id)
    
    def get_result(self, result_id: int):
        """Retourne un résultat d'évaluation par son identifiant."""
        return self.result_repository.get_by_id(result_id)