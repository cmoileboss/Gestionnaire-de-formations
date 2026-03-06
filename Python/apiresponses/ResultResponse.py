from pydantic import BaseModel, ConfigDict
from datetime import datetime

class ResultResponse(BaseModel):
    """Pydantic response model for an evaluation result.

    Returned by result endpoints.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Identifier of the user.
    user_id: int
    #: Identifier of the evaluation.
    evaluation_id: int
    #: Score obtained in the evaluation.
    score: float
    #: Whether the evaluation was passed.
    success: bool
    #: Date of the result.
    date: datetime = None
