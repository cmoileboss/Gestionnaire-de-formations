from pydantic import BaseModel, ConfigDict
from typing import Optional

class UserResponse(BaseModel):
    """Pydantic response model for a user.

    Returned by user endpoints to expose non-sensitive user data.
    The password hash is intentionally excluded.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Unique identifier of the user.
    user_id: int
    #: Email address of the user.
    email: str
    #: Optional postal address of the user.
    address: Optional[str] = None
