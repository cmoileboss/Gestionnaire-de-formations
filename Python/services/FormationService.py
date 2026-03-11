from sqlalchemy.orm import Session

from repositories.FormationRepository import FormationRepository
from repositories.ModuleRepository import ModuleRepository
from repositories.ModuleUsageRepository import ModuleUsageRepository
from models.Formation import Formation
from models.Module import Module
from exceptions import NotFoundError, DuplicateError


class FormationService:
    def __init__(self, db: Session):
        self.formation_repository = FormationRepository(db)
        self.module_repository = ModuleRepository(db)
        self.module_usage_repository = ModuleUsageRepository(db)


    def get_all_formations(self) -> list[Formation]:
        """Retourne la liste de toutes les formations."""
        return self.formation_repository.get_all()

    def get_formation(self, formation_id: int) -> Formation:
        """Retourne une formation par son identifiant, ou lève NotFoundError."""
        formation = self.formation_repository.get_by_id(formation_id)
        if formation is None:
            raise NotFoundError("Formation", formation_id)
        return formation

    def create_formation(self, name: str, description: str = None) -> Formation:
        """Crée une nouvelle formation."""
        return self.formation_repository.create(name, description)

    def update_formation(self, formation_id: int, name: str = None, description: str = None) -> Formation:
        """Met à jour une formation existante. Lève NotFoundError si introuvable."""
        updated = self.formation_repository.update(formation_id, name, description)
        if updated is None:
            raise NotFoundError("Formation", formation_id)
        return updated

    def delete_formation(self, formation_id: int) -> None:
        """Supprime une formation. Lève NotFoundError si introuvable."""
        success = self.formation_repository.delete(formation_id)
        if not success:
            raise NotFoundError("Formation", formation_id)


    def get_formation_modules(self, formation_id: int) -> list[Module]:
        """Retourne tous les modules associés à une formation."""
        # Vérifier que la formation existe
        formation = self.formation_repository.get_by_id(formation_id)
        if formation is None:
            raise NotFoundError("Formation", formation_id)
        
        usages = self.module_usage_repository.get_by_formation(formation_id)
        modules = []
        for usage in usages:
            module = self.module_repository.get_by_id(usage["module_id"])
            if module:
                modules.append(module)
        return modules

    def add_module_to_formation(self, formation_id: int, module_id: int) -> None:
        """
        Associe un module à une formation.
        Lève NotFoundError si la formation ou le module n'existe pas.
        Lève DuplicateError si le module est déjà lié.
        """
        # Vérifier que la formation existe
        formation = self.formation_repository.get_by_id(formation_id)
        if formation is None:
            raise NotFoundError("Formation", formation_id)
        
        # Vérifier que le module existe
        module = self.module_repository.get_by_id(module_id)
        if module is None:
            raise NotFoundError("Module", module_id)
        
        existing = self.module_usage_repository.get(module_id, formation_id)
        if existing:
            raise DuplicateError(f"Le module {module_id} est déjà associé à la formation {formation_id}")
        self.module_usage_repository.create(module_id, formation_id)

    def remove_module_from_formation(self, formation_id: int, module_id: int) -> None:
        """Supprime le lien entre un module et une formation. Lève NotFoundError si non trouvé."""
        # Vérifier que la formation existe
        formation = self.formation_repository.get_by_id(formation_id)
        if formation is None:
            raise NotFoundError("Formation", formation_id)
        
        # Vérifier que le module existe
        module = self.module_repository.get_by_id(module_id)
        if module is None:
            raise NotFoundError("Module", module_id)
        
        success = self.module_usage_repository.delete(module_id, formation_id)
        if not success:
            raise NotFoundError("Association module-formation", f"{module_id}/{formation_id}")
