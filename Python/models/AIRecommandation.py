from sqlalchemy import CheckConstraint, Integer, Identity, SmallInteger, DateTime, ForeignKeyConstraint, Index
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base

class AIRecommandation(Base):
    """
    Modèle SQLAlchemy représentant une recommandation IA pour une session.
    Contient la session, la date et le niveau de confiance.
    """
    __tablename__ = 'AIRecommandation'
    __table_args__ = (
        ForeignKeyConstraint(['session_id'], ['Session.session_id'], name='FK__AIRecommandation__session_id'),
        Index('IX_AIRecommandation_session_id', 'session_id', mssql_clustered=False, mssql_include=[]),
        CheckConstraint('confidence > 0 AND confidence < 100', name='CK_Recommandation_Confidence')
    )

    recommendation_id = mapped_column(Integer, Identity(start=1, increment=1), primary_key=True) #: Identifiant recommandation
    session_id = mapped_column(Integer, nullable=False)                                          #: Identifiant session
    date = mapped_column(DateTime, nullable=False)                                               #: Date de la recommandation
    confidence = mapped_column(SmallInteger, nullable=False)                                     #: Niveau de confiance (1-99)

    session = relationship('Session', back_populates='AIRecommandation')                         #: Session concernée
