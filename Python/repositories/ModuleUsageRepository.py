from sqlalchemy import delete, insert, select
from sqlalchemy.orm import Session
from models.ModuleUsage import t_ModuleUsage


class ModuleUsageRepository:
    """
    Repository SQLAlchemy pour la gestion de la table d'association ModuleUsage.
    Gère les liens many-to-many entre modules et formations.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, module_id: int, formation_id: int) -> dict:
        """
        Crée un lien entre un module et une formation.
        :param module_id: Identifiant module
        :param formation_id: Identifiant formation
        :return: Dictionnaire contenant module_id et formation_id
        """
        stmt = insert(t_ModuleUsage).values(module_id=module_id, formation_id=formation_id)
        self.db.execute(stmt)
        self.db.commit()
        return {'module_id': module_id, 'formation_id': formation_id}

    def get(self, module_id: int, formation_id: int) -> dict | None:
        """
        Récupère un lien spécifique entre un module et une formation.
        :param module_id: Identifiant module
        :param formation_id: Identifiant formation
        :return: Dictionnaire du lien ou None
        """
        stmt = select(t_ModuleUsage).where(
            t_ModuleUsage.c.module_id == module_id,
            t_ModuleUsage.c.formation_id == formation_id
        )
        result = self.db.execute(stmt).first()
        return dict(result) if result else None

    def get_by_module(self, module_id: int) -> list[dict]:
        """
        Récupère toutes les formations associées à un module donné.
        :param module_id: Identifiant module
        :return: Liste de dictionnaires de liens
        """
        stmt = select(t_ModuleUsage).where(t_ModuleUsage.c.module_id == module_id)
        result = self.db.execute(stmt).fetchall()
        return [dict(row) for row in result]

    def get_by_formation(self, formation_id: int) -> list[dict]:
        """
        Récupère tous les modules associés à une formation donnée.
        :param formation_id: Identifiant formation
        :return: Liste de dictionnaires de liens
        """
        stmt = select(t_ModuleUsage).where(t_ModuleUsage.c.formation_id == formation_id)
        result = self.db.execute(stmt).fetchall()
        return [dict(row) for row in result]

    def get_all(self) -> list[dict]:
        """
        Récupère tous les liens module-formation avec pagination.
        :param skip: Décalage de départ
        :param limit: Nombre maximum de résultats
        :return: Liste de dictionnaires de liens
        """
        stmt = select(t_ModuleUsage)
        result = self.db.execute(stmt).fetchall()
        return [dict(row) for row in result]

    def delete(self, module_id: int, formation_id: int) -> bool:
        """
        Supprime le lien entre un module et une formation.
        :param module_id: Identifiant module
        :param formation_id: Identifiant formation
        :return: True si supprimé, False sinon
        """
        stmt = delete(t_ModuleUsage).where(
            t_ModuleUsage.c.module_id == module_id,
            t_ModuleUsage.c.formation_id == formation_id
        )
        result = self.db.execute(stmt)
        self.db.commit()
        return result.rowcount > 0
