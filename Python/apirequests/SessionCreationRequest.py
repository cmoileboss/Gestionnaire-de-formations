from pydantic import BaseModel, ConfigDict

class SessionCreationRequest(BaseModel):
    """Request body for creating or updating a training session.

    :param formation_id: ID of the formation this session belongs to.
    :param start_date: Session start date (ISO 8601 string).
    :param end_date: Session end date (ISO 8601 string).
    :param place: Name or description of the session location.
    :param address: Full postal address of the session venue.
    """

    model_config = ConfigDict(extra="forbid")

    #: ID of the formation this session belongs to.
    formation_id: int
    #: Session start date (ISO 8601 string).
    start_date: str
    #: Session end date (ISO 8601 string).
    end_date: str
    #: Name or description of the session location.
    place: str
    #: Full postal address of the session venue.
    address: str