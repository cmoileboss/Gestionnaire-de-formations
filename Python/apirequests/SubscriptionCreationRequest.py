from pydantic import BaseModel, ConfigDict
from datetime import datetime

class SubscriptionCreationRequest(BaseModel):
    """Request body for creating a subscription (enrolling a user in a session).

    :param user_id: ID of the user to subscribe.
    :param session_id: ID of the session to subscribe the user to.
    :param subscription_date: Date and time of the subscription.
    """

    model_config = ConfigDict(extra="forbid")

    #: ID of the user to subscribe.
    user_id: int
    #: ID of the session to subscribe the user to.
    session_id: int
    #: Date and time when the subscription is created.
    subscription_date: datetime
