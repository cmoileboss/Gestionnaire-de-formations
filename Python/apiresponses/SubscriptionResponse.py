from pydantic import BaseModel, ConfigDict
from datetime import datetime

class SubscriptionResponse(BaseModel):
    """Pydantic response model for a session subscription.

    Represents the enrollment of a user in a training session.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Identifier of the subscribed user.
    user_id: int
    #: Identifier of the session the user is subscribed to.
    session_id: int
    #: Date and time when the subscription was created.
    subscription_date: datetime
