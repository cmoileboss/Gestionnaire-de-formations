from sqlalchemy import Integer, Identity, Unicode, DateTime, ForeignKeyConstraint, Index
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base

class Session(Base):
    """
    Modèle SQLAlchemy représentant une session de formation.
    Contient les dates, le lieu et les relations avec formation, recommandations IA et abonnements.
    """
    __tablename__ = 'Session'
    __table_args__ = (
        ForeignKeyConstraint(['formation_id'], ['Formation.formation_id'], name='FK__Session__formati__5165187F'),
        Index('IX_Session_formation_id', 'formation_id', mssql_clustered=False, mssql_include=[]),
    )

    session_id = mapped_column(Integer, Identity(start=1, increment=1), primary_key=True) #: Identifiant session
    formation_id = mapped_column(Integer, nullable=False)                                 #: Identifiant formation
    start_date = mapped_column(DateTime, nullable=False)                                 #: Date de début
    end_date = mapped_column(DateTime, nullable=False)                                   #: Date de fin
    place = mapped_column(Unicode(255, 'French_CI_AS'), nullable=False)                  #: Lieu

    formation = relationship('Formation', back_populates='Session')                      #: Formation liée
    AIRecommandation = relationship('AIRecommandation', back_populates='session')        #: Recommandations IA
    Subscription = relationship('Subscription', back_populates='session')                #: Abonnements à la session
