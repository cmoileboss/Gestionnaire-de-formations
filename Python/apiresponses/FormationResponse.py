from pydantic import BaseModel, ConfigDict

class FormationResponse(BaseModel):
    """Pydantic response model for a formation.

    Returned by formation endpoints.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Unique identifier of the formation.
    formation_id: int
    #: Title of the formation.
    title: str
    #: Optional description of the formation.
    description: str
