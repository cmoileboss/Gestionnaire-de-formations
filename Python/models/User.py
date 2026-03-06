from sqlalchemy import Integer, Identity, Unicode
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base
from sqlalchemy import Index

class User(Base):
    """
    Modèle SQLAlchemy représentant un utilisateur de la plateforme.
    Contient les informations d'identification et les relations avec les abonnements et résultats.
    """
    __tablename__ = 'Users'
    __table_args__ = (
        Index('UQ__Users__AB6E61643453F3CB', 'email', mssql_clustered=False, mssql_include=[], unique=True),
    )

    user_id = mapped_column(Integer, Identity(start=1, increment=1), primary_key=True)  #: Identifiant unique
    password_hash = mapped_column(Unicode(100, 'French_CI_AS'), nullable=False)        #: Mot de passe hashé
    email = mapped_column(Unicode(255, 'French_CI_AS'), nullable=False)                #: Email unique
    address = mapped_column(Unicode(255, 'French_CI_AS'))                              #: Adresse postale

    Subscription = relationship('Subscription', back_populates='user')                 #: Abonnements de l'utilisateur
    results = relationship('Result', back_populates='user', cascade='all, delete-orphan') #: Résultats de l'utilisateur
