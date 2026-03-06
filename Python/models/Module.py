from sqlalchemy import Integer, Identity, Unicode
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base
from .ModuleUsage import t_ModuleUsage

class Module(Base):
    """
    Modèle SQLAlchemy représentant un module de formation.
    Contient le titre, le sujet, la description et les relations avec formation et évaluation.
    """
    __tablename__ = 'Module'

    module_id = mapped_column(Integer, Identity(start=1, increment=1), primary_key=True) #: Identifiant module
    title = mapped_column(Unicode(255, 'French_CI_AS'), nullable=False)                  #: Titre du module
    subject = mapped_column(Unicode(255, 'French_CI_AS'), nullable=False)                #: Sujet du module
    description = mapped_column(Unicode(collation='French_CI_AS'), nullable=False)       #: Description

    formation = relationship('Formation', secondary=t_ModuleUsage, back_populates='module') #: Formations associées
    Evaluation = relationship('Evaluation', back_populates='module')                         #: Évaluations du module