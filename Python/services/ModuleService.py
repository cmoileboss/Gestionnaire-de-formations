from sqlalchemy.orm import Session

from repositories.ModuleRepository import ModuleRepository
from models.Module import Module
from exceptions import NotFoundError


class ModuleService:
    def __init__(self, db: Session):
        self.module_repository = ModuleRepository(db)


    def get_all_modules(self) -> list[Module]:
        """Retourne la liste de tous les modules."""
        return self.module_repository.get_all()

    def get_module(self, module_id: int) -> Module:
        """Retourne un module par son identifiant, ou lève NotFoundError."""
        module = self.module_repository.get_by_id(module_id)
        if module is None:
            raise NotFoundError("Module", module_id)
        return module

    def create_module(self, title: str, subject: str = None, description: str = None) -> Module:
        """Crée un nouveau module."""
        return self.module_repository.create(title, subject, description)

    def update_module(self, module_id: int, title: str = None, subject: str = None, description: str = None) -> Module:
        """Met à jour un module existant. Lève NotFoundError si introuvable."""
        updated = self.module_repository.update(module_id, title, subject, description)
        if updated is None:
            raise NotFoundError("Module", module_id)
        return updated

    def delete_module(self, module_id: int) -> None:
        """Supprime un module. Lève NotFoundError si introuvable."""
        success = self.module_repository.delete(module_id)
        if not success:
            raise NotFoundError("Module", module_id)
