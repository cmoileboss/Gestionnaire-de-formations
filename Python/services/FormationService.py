from sqlalchemy.orm import Session

from repositories.FormationRepository import FormationRepository
from repositories.ModuleRepository import ModuleRepository
from repositories.ModuleUsageRepository import ModuleUsageRepository
from models.Formation import Formation
from models.Module import Module


class FormationService:
    def __init__(self, db: Session):
        self.formation_repository = FormationRepository(db)
        self.module_repository = ModuleRepository(db)
        self.module_usage_repository = ModuleUsageRepository(db)


    def get_all_formations(self) -> list[Formation]:
        """Retourne la liste de toutes les formations."""
        return self.formation_repository.get_all()

    def get_formation(self, formation_id: int) -> Formation | None:
        """Retourne une formation par son identifiant, ou None."""
        return self.formation_repository.get_by_id(formation_id)

    def create_formation(self, name: str, description: str = None) -> Formation:
        """Crée une nouvelle formation."""
        return self.formation_repository.create(name, description)

    def update_formation(self, formation_id: int, name: str = None, description: str = None) -> Formation | None:
        """Met à jour une formation existante. Retourne None si introuvable."""
        return self.formation_repository.update(formation_id, name, description)

    def delete_formation(self, formation_id: int) -> bool:
        """Supprime une formation. Retourne True si supprimée."""
        return self.formation_repository.delete(formation_id)


    def get_formation_modules(self, formation_id: int) -> list[Module]:
        """Retourne tous les modules associés à une formation."""
        usages = self.module_usage_repository.get_by_formation(formation_id)
        modules = []
        for usage in usages:
            module = self.module_repository.get_by_id(usage["module_id"])
            if module:
                modules.append(module)
        return modules

    def add_module_to_formation(self, formation_id: int, module_id: int):
        """
        Associe un module à une formation.
        Retourne None si le module n'existe pas.
        Lève ValueError si le module est déjà lié.
        """
        module = self.module_repository.get_by_id(module_id)
        if module is None:
            return None
        existing = self.module_usage_repository.get(module_id, formation_id)
        if existing:
            raise ValueError("Module already added to this formation")
        self.module_usage_repository.create(module_id, formation_id)
        return True

    def remove_module_from_formation(self, formation_id: int, module_id: int) -> bool:
        """Supprime le lien entre un module et une formation. Retourne True si supprimé."""
        return self.module_usage_repository.delete(module_id, formation_id)
