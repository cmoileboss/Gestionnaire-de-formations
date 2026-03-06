from sqlalchemy import Integer, Identity, Unicode, DateTime, ForeignKeyConstraint, Index
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base

class Evaluation(Base):
    """
    Modèle SQLAlchemy représentant une évaluation d'un module.
    Contient les dates, le lieu, le module associé et les résultats.
    """
    __tablename__ = 'Evaluation'
    __table_args__ = (
        ForeignKeyConstraint(['module_id'], ['Module.module_id'], ondelete='CASCADE', name='FK__Evaluation__modul_id'),
        Index('IX_Evaluation_module_id', 'module_id', mssql_clustered=False, mssql_include=[]),
    )

    evaluation_id = mapped_column(Integer, Identity(start=1, increment=1), primary_key=True) #: Identifiant évaluation
    start_date = mapped_column(DateTime, nullable=False)                                     #: Date de début
    end_date = mapped_column(DateTime, nullable=False)                                       #: Date de fin
    place = mapped_column(Unicode(255, 'French_CI_AS'), nullable=False)                      #: Lieu
    module_id = mapped_column(Integer, nullable=False)                                       #: Identifiant module

    module = relationship('Module', back_populates='Evaluation')                             #: Module concerné
    results = relationship('Result', back_populates='evaluation', cascade='all, delete-orphan') #: Résultats de l'évaluation