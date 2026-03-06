from sqlalchemy import Integer, Float, Boolean, DateTime, ForeignKeyConstraint, Index
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base

class Result(Base):
    """
    Modèle SQLAlchemy représentant le résultat d'une évaluation pour un utilisateur.
    Clé primaire composite (user_id, evaluation_id).
    """
    __tablename__ = 'Result'
    __table_args__ = (
        ForeignKeyConstraint(['user_id'], ['Users.user_id'], ondelete='CASCADE', name='FK_Result_user_id'),
        ForeignKeyConstraint(['evaluation_id'], ['Evaluation.evaluation_id'], ondelete='CASCADE', name='FK_Result_evaluation_id'),
        Index('IX_Result_user_id', 'user_id', mssql_clustered=False, mssql_include=[]),
        Index('IX_Result_evaluation_id', 'evaluation_id', mssql_clustered=False, mssql_include=[]),
    )

    user_id = mapped_column(Integer, primary_key=True)         #: Identifiant utilisateur
    evaluation_id = mapped_column(Integer, primary_key=True)   #: Identifiant évaluation
    score = mapped_column(Float(53), nullable=False)           #: Score obtenu
    success = mapped_column(Boolean, nullable=False)           #: Succès ou échec
    date = mapped_column(DateTime, nullable=False)             #: Date du résultat

    user = relationship('User', back_populates='results')      #: Utilisateur concerné
    evaluation = relationship('Evaluation', back_populates='results') #: Évaluation concernée

