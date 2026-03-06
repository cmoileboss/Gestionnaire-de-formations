from sqlalchemy import Integer, Identity, Unicode
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base
from .ModuleUsage import t_ModuleUsage

class Formation(Base):
    """
    Modèle SQLAlchemy représentant une formation.
    Contient le titre, la description et les relations avec modules et sessions.
    """
    __tablename__ = 'Formation'

    formation_id = mapped_column(Integer, Identity(start=1, increment=1), primary_key=True) #: Identifiant formation
    title = mapped_column(Unicode(255, 'French_CI_AS'), nullable=False)                     #: Titre de la formation
    description = mapped_column(Unicode(collation='French_CI_AS'), nullable=False)          #: Description

    module = relationship('Module', secondary=t_ModuleUsage, back_populates='formation')   #: Modules associés
    Session = relationship('Session', back_populates='formation')                          #: Sessions de la formation
