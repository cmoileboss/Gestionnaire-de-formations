from sqlalchemy import Integer, DateTime, ForeignKeyConstraint, Index
from sqlalchemy.orm import mapped_column, relationship
from database_connection import Base

class Subscription(Base):
    """
    Modèle SQLAlchemy représentant l'abonnement d'un utilisateur à une session.
    Clé primaire composite (user_id, session_id).
    """
    __tablename__ = 'Subscription'
    __table_args__ = (
        ForeignKeyConstraint(['session_id'], ['Session.session_id'], ondelete='CASCADE', name='FK__Subscript__sessi__5BE2A6F2'),
        ForeignKeyConstraint(['user_id'], ['Users.user_id'], ondelete='CASCADE', name='FK__Subscript__user___5AEE82B9'),
        Index('IX_Subscription_session_id', 'session_id', mssql_clustered=False, mssql_include=[]),
    )

    user_id = mapped_column(Integer, primary_key=True)           #: Identifiant utilisateur
    session_id = mapped_column(Integer, primary_key=True)        #: Identifiant session
    subscription_date = mapped_column(DateTime, nullable=False)  #: Date d'abonnement

    session = relationship('Session', back_populates='Subscription') #: Session liée
    user = relationship('User', back_populates='Subscription')       #: Utilisateur lié
