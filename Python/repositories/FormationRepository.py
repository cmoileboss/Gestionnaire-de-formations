from sqlalchemy.orm import Session
from models.Formation import Formation


class FormationRepository:
    """
    Repository SQLAlchemy pour la gestion des formations.
    Fournit des méthodes CRUD et de recherche sur les formations.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, title: str, description: str = None) -> Formation:
        """
        Crée une nouvelle formation.
        :param title: Titre de la formation
        :param description: Description de la formation (optionnel)
        :return: La formation créée
        """
        formation = Formation(
            title=title,
            description=description
        )
        self.db.add(formation)
        self.db.commit()
        self.db.refresh(formation)
        return formation

    def get_by_id(self, formation_id: int) -> Formation:
        """
        Récupère une formation par son identifiant unique.
        :param formation_id: Identifiant formation
        :return: La formation ou None
        """
        return self.db.query(Formation).filter(
            Formation.formation_id == formation_id
        ).first()

    def get_by_title(self, title: str) -> list[Formation]:
        """
        Recherche les formations dont le titre contient la chaîne donnée.
        :param title: Titre à rechercher
        :return: Liste de formations
        """
        return self.db.query(Formation).filter(
            Formation.title.contains(title)
        ).all()

    def get_all(self) -> list[Formation]:
        """
        Récupère toutes les formations.
        :return: Liste de formations
        """
        return self.db.query(Formation).all()

    def update(self, formation_id: int, title: str = None, description: str = None) -> Formation:
        """
        Met à jour les informations d'une formation existante.
        :param formation_id: Identifiant formation
        :param title: Nouveau titre (optionnel)
        :param description: Nouvelle description (optionnel)
        :return: La formation mise à jour ou None
        """
        formation = self.get_by_id(formation_id)
        if formation:
            if title is not None:
                formation.title = title
            if description is not None:
                formation.description = description
            self.db.commit()
            self.db.refresh(formation)
        return formation

    def delete(self, formation_id: int) -> bool:
        """
        Supprime une formation de la base.
        :param formation_id: Identifiant formation
        :return: True si supprimé, False sinon
        """
        formation = self.get_by_id(formation_id)
        if formation:
            self.db.delete(formation)
            self.db.commit()
            return True
        return False
