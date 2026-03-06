from pydantic import BaseModel, ConfigDict
from datetime import datetime

class EvaluationResponse(BaseModel):
    """Pydantic response model for an evaluation.

    Returned by evaluation endpoints.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Unique identifier of the evaluation.
    evaluation_id: int
    #: Start datetime of the evaluation.
    start_date: datetime = None
    #: End datetime of the evaluation.
    end_date: datetime = None
    #: Location where the evaluation takes place.
    place: str = None
    #: Identifier of the module this evaluation is associated with.
    module_id: int = None
