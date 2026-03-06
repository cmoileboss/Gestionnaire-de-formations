from sqlalchemy.orm import Session
from models.User import User


class UserRepository:
    """
    Repository SQLAlchemy pour la gestion des utilisateurs.
    Fournit des méthodes CRUD pour manipuler les objets User en base.
    """
    def __init__(self, db: Session):
        """
        Initialise le repository avec une session SQLAlchemy.
        :param db: Session SQLAlchemy
        """
        self.db = db

    def create(self, email: str, password: str, address: str = None) -> User:
        """
        Crée un nouvel utilisateur et le persiste en base.
        :param email: Email de l'utilisateur
        :param password: Mot de passe hashé
        :param address: Adresse postale (optionnelle)
        :return: L'utilisateur créé
        """
        user = User(
            email=email,
            password_hash=password,
            address=address
        )
        self.db.add(user)
        self.db.commit()
        self.db.refresh(user)
        return user

    def get_by_id(self, user_id: int) -> User:
        """
        Récupère un utilisateur par son identifiant unique.
        :param user_id: Identifiant utilisateur
        :return: L'utilisateur ou None
        """
        return self.db.query(User).filter(User.user_id == user_id).first()

    def get_by_email(self, email: str) -> User:
        """
        Récupère un utilisateur par son email.
        :param email: Email utilisateur
        :return: L'utilisateur ou None
        """
        return self.db.query(User).filter(User.email == email).first()

    def get_all(self) -> list[User]:
        """
        Récupère tous les utilisateurs présents en base.
        :return: Liste d'utilisateurs
        """
        return self.db.query(User).all()

    def update(self, user_id: int, email: str = None, password: str = None, address: str = None) -> User:
        """
        Met à jour les informations d'un utilisateur existant.
        :param user_id: Identifiant utilisateur
        :param email: Nouvel email (optionnel)
        :param password: Nouveau mot de passe hashé (optionnel)
        :param address: Nouvelle adresse (optionnelle)
        :return: L'utilisateur mis à jour ou None
        """
        user = self.get_by_id(user_id)
        if user:
            if email is not None:
                user.email = email
            if password is not None:
                user.password_hash = password
            if address is not None:
                user.address = address
            self.db.commit()
            self.db.refresh(user)
        return user

    def delete(self, user_id: int) -> bool:
        """
        Supprime un utilisateur de la base.
        :param user_id: Identifiant utilisateur
        :return: True si supprimé, False sinon
        """
        user = self.get_by_id(user_id)
        if user:
            self.db.delete(user)
            self.db.commit()
            return True
        return False
