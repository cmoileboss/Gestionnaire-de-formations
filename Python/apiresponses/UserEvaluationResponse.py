from pydantic import BaseModel, ConfigDict

class UserEvaluationResponse(BaseModel):
    """Pydantic response model for a user-evaluation enrollment.

    Represents the association between a user and an evaluation they are enrolled in.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Unique identifier of the user-evaluation record.
    user_evaluation_id: int
    #: Identifier of the evaluation.
    evaluation_id: int
    #: Identifier of the enrolled user.
    user_id: int
