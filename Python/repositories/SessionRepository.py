from sqlalchemy.orm import Session
from models.Session import Session as SessionModel
from datetime import datetime


class SessionRepository:
    """
    Repository SQLAlchemy pour la gestion des sessions de formation.
    Fournit des méthodes CRUD et de recherche sur les sessions.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, formation_id: int, start_date: datetime, end_date: datetime, 
               place: str) -> SessionModel:
        """
        Crée une nouvelle session de formation.
        :param formation_id: Identifiant de la formation
        :param start_date: Date de début
        :param end_date: Date de fin
        :param place: Lieu de la session
        :return: La session créée
        """
        session = SessionModel(
            formation_id=formation_id,
            start_date=start_date,
            end_date=end_date,
            place=place
        )
        self.db.add(session)
        self.db.commit()
        self.db.refresh(session)
        return session

    def get_by_id(self, session_id: int) -> SessionModel:
        """
        Récupère une session par son identifiant unique.
        :param session_id: Identifiant session
        :return: La session ou None
        """
        return self.db.query(SessionModel).filter(
            SessionModel.session_id == session_id
        ).first()

    def get_by_formation(self, formation_id: int) -> list[SessionModel]:
        """
        Récupère toutes les sessions d'une formation donnée.
        :param formation_id: Identifiant formation
        :return: Liste de sessions
        """
        return self.db.query(SessionModel).filter(
            SessionModel.formation_id == formation_id
        ).all()

    def get_by_place(self, place: str) -> list[SessionModel]:
        """
        Récupère toutes les sessions ayant lieu à un endroit donné.
        :param place: Lieu recherché
        :return: Liste de sessions
        """
        return self.db.query(SessionModel).filter(
            SessionModel.place.contains(place)
        ).all()

    def get_active_sessions(self) -> list[SessionModel]:
        """
        Récupère toutes les sessions actuellement en cours.
        :return: Liste de sessions actives
        """
        now = datetime.utcnow()
        return self.db.query(SessionModel).filter(
            SessionModel.start_date <= now,
            SessionModel.end_date >= now
        ).all()

    def get_all(self) -> list[SessionModel]:
        """
        Récupère toutes les sessions avec pagination.
        :param skip: Décalage de départ
        :param limit: Nombre maximum de résultats
        :return: Liste de sessions
        """
        return self.db.query(SessionModel).all()

    def update(self, session_id: int, formation_id: int = None, start_date: datetime = None,
               end_date: datetime = None, place: str = None) -> SessionModel:
        """
        Met à jour les informations d'une session existante.
        :param session_id: Identifiant session
        :param formation_id: Nouvel identifiant formation (optionnel)
        :param start_date: Nouvelle date de début (optionnel)
        :param end_date: Nouvelle date de fin (optionnel)
        :param place: Nouveau lieu (optionnel)
        :return: La session mise à jour ou None
        """
        session = self.get_by_id(session_id)
        if session:
            if formation_id is not None:
                session.formation_id = formation_id
            if start_date is not None:
                session.start_date = start_date
            if end_date is not None:
                session.end_date = end_date
            if place is not None:
                session.place = place
            self.db.commit()
            self.db.refresh(session)
        return session

    def delete(self, session_id: int) -> bool:
        """Supprimer une session"""
        session = self.get_by_id(session_id)
        if session:
            self.db.delete(session)
            self.db.commit()
            return True
        return False
