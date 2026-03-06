from sqlalchemy import Table, Column, Integer, ForeignKeyConstraint, PrimaryKeyConstraint, Index
from database_connection import Base

t_ModuleUsage = Table(
    'ModuleUsage', Base.metadata,

    Column('module_id', Integer, primary_key=True),
    Column('formation_id', Integer, primary_key=True),

    ForeignKeyConstraint(['formation_id'], ['Formation.formation_id'], name='FK__ModuleUsage__formation_id'),
    ForeignKeyConstraint(['module_id'], ['Module.module_id'], name='FK__ModuleUsage__module_id'),
    
    PrimaryKeyConstraint('module_id', 'formation_id', name='PK__ModuleUsage'),

    Index('IX_ModuleUsage_formation_id', 'formation_id', mssql_clustered=False, mssql_include=[])
)
"""
Table d'association SQLAlchemy entre Module et Formation.
Permet de gérer la relation many-to-many entre modules et formations.
"""
