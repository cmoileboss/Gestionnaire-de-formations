from pydantic import BaseModel, ConfigDict
from datetime import datetime

class EvaluationCreationRequest(BaseModel):
    """Request body for creating or updating an evaluation.

    :param start_date: Start datetime of the evaluation.
    :param end_date: End datetime of the evaluation.
    :param place: Location where the evaluation takes place.
    :param module_id: ID of the module this evaluation is associated with.
    """

    model_config = ConfigDict(extra="forbid")

    #: Start datetime of the evaluation.
    start_date: datetime
    #: End datetime of the evaluation.
    end_date: datetime
    #: Location where the evaluation takes place.
    place: str
    #: ID of the module this evaluation is associated with.
    module_id: int
