from pydantic import BaseModel, ConfigDict

class FormationCreationRequest(BaseModel):
    """
    Modèle de requête pour la création d'une formation.
    Contient le nom et la description de la formation à créer.
    """
    model_config = ConfigDict(extra="forbid")

    name: str         #: Nom de la formation
    description: str  #: Description de la formation
