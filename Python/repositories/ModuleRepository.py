from sqlalchemy.orm import Session
from models.Module import Module


class ModuleRepository:
    """
    Repository SQLAlchemy pour la gestion des modules de formation.
    Fournit des méthodes CRUD et de recherche sur les modules.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, title: str = None, subject: str = None, description: str = None) -> Module:
        """
        Crée un nouveau module de formation.
        :param title: Titre du module
        :param subject: Sujet du module
        :param description: Description du module
        :return: Le module créé
        """
        module = Module(
            title=title,
            subject=subject,
            description=description
        )
        self.db.add(module)
        self.db.commit()
        self.db.refresh(module)
        return module

    def get_by_id(self, module_id: int) -> Module:
        """
        Récupère un module par son identifiant unique.
        :param module_id: Identifiant module
        :return: Le module ou None
        """
        return self.db.query(Module).filter(
            Module.module_id == module_id
        ).first()

    def get_by_subject(self, subject: str) -> list[Module]:
        """
        Recherche les modules dont le sujet contient la chaîne donnée.
        :param subject: Sujet à rechercher
        :return: Liste de modules
        """
        return self.db.query(Module).filter(
            Module.subject.contains(subject)
        ).all()

    def get_by_title(self, title: str) -> list[Module]:
        """
        Recherche les modules dont le titre contient la chaîne donnée.
        :param title: Titre à rechercher
        :return: Liste de modules
        """
        return self.db.query(Module).filter(
            Module.title.contains(title)
        ).all()

    def get_all(self) -> list[Module]:
        """
        Récupère tous les modules avec pagination.
        :param skip: Décalage de départ
        :param limit: Nombre maximum de résultats
        :return: Liste de modules
        """
        return self.db.query(Module).all()

    def update(self, module_id: int, title: str = None, subject: str = None, 
               description: str = None) -> Module:
        """
        Met à jour les informations d'un module existant.
        :param module_id: Identifiant module
        :param title: Nouveau titre (optionnel)
        :param subject: Nouveau sujet (optionnel)
        :param description: Nouvelle description (optionnel)
        :return: Le module mis à jour ou None
        """
        module = self.get_by_id(module_id)
        if module:
            if title is not None:
                module.title = title
            if subject is not None:
                module.subject = subject
            if description is not None:
                module.description = description
            self.db.commit()
            self.db.refresh(module)
        return module

    def delete(self, module_id: int) -> bool:
        """
        Supprime un module de la base.
        :param module_id: Identifiant module
        :return: True si supprimé, False sinon
        """
        module = self.get_by_id(module_id)
        if module:
            self.db.delete(module)
            self.db.commit()
            return True
        return False
