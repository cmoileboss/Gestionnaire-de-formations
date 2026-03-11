from pydantic import BaseModel, ConfigDict
from datetime import datetime

class SessionResponse(BaseModel):
    """Pydantic response model for a training session.

    Returned by session endpoints.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Unique identifier of the session.
    session_id: int
    #: Identifier of the associated formation.
    formation_id: int
    #: Start date of the session (ISO 8601 string).
    start_date: datetime
    #: End date of the session (ISO 8601 string).
    end_date: datetime
    #: Name or description of the session location.
    place: str