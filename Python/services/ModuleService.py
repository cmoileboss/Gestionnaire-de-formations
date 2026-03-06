from sqlalchemy.orm import Session

from repositories.ModuleRepository import ModuleRepository
from models.Module import Module


class ModuleService:
    def __init__(self, db: Session):
        self.module_repository = ModuleRepository(db)


    def get_all_modules(self) -> list[Module]:
        """Retourne la liste de tous les modules."""
        return self.module_repository.get_all()

    def get_module(self, module_id: int) -> Module | None:
        """Retourne un module par son identifiant, ou None."""
        return self.module_repository.get_by_id(module_id)

    def create_module(self, title: str, subject: str = None, description: str = None) -> Module:
        """Crée un nouveau module."""
        return self.module_repository.create(title, subject, description)

    def update_module(self, module_id: int, title: str = None, subject: str = None, description: str = None) -> Module | None:
        """Met à jour un module existant. Retourne None si introuvable."""
        return self.module_repository.update(module_id, title, subject, description)

    def delete_module(self, module_id: int) -> bool:
        """Supprime un module. Retourne True si supprimé."""
        return self.module_repository.delete(module_id)
