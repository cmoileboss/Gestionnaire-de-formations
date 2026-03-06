"""Router for subscription endpoints: CRUD operations on user-session enrollments."""

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import Annotated

from database_connection import get_db

from models.Subscription import Subscription
from models.User import User

from services.SubscriptionService import SubscriptionService
from services.SecurityService import SecurityService

from apirequests.SubscriptionCreationRequest import SubscriptionCreationRequest
from apiresponses.SubscriptionResponse import SubscriptionResponse


def get_subscription_service(db: Session = Depends(get_db)) -> SubscriptionService:
    """Get a SubscriptionService instance with the database session"""
    return SubscriptionService(db)

subscription_router = APIRouter(prefix="/subscriptions", tags=["Subscriptions"], dependencies=[Depends(SecurityService.get_current_user)])
SubscriptionServiceDep = Annotated[SubscriptionService, Depends(get_subscription_service)]

@subscription_router.get("/", response_model=list[SubscriptionResponse])
def read_subscriptions(subscription_service: SubscriptionServiceDep) -> list[Subscription]:
    """Return the list of all subscriptions.

    :param subscription_service: Injected subscription service.
    :return: List of all Subscription records.
    """
    return subscription_service.get_all_subscriptions()

@subscription_router.get("/user/{user_id}", response_model=list[SubscriptionResponse])
def read_subscriptions_by_user(user_id: int, subscription_service: SubscriptionServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> list[Subscription]:
    """Return all subscriptions belonging to a specific user.

    :param user_id: Primary key of the user whose subscriptions to retrieve.
    :param subscription_service: Injected subscription service.
    :param current_user: Currently authenticated user.
    :return: List of Subscription records for the given user.
    :raises HTTPException 403: If the current user is not the owner.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only access your own subscriptions")
    return subscription_service.get_subscriptions_by_user(user_id)

@subscription_router.get("/session/{session_id}", response_model=list[SubscriptionResponse])
def read_subscriptions_by_session(session_id: int, subscription_service: SubscriptionServiceDep) -> list[Subscription]:
    """Return all subscriptions for a specific training session.

    :param session_id: Primary key of the session whose subscriptions to retrieve.
    :param subscription_service: Injected subscription service.
    :return: List of Subscription records for the given session.
    """
    return subscription_service.get_subscriptions_by_session(session_id)

@subscription_router.get("/{user_id}/{session_id}", response_model=SubscriptionResponse)
def read_subscription(user_id: int, session_id: int, subscription_service: SubscriptionServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> Subscription:
    """Return a specific subscription by user and session identifiers.

    :param user_id: Primary key of the subscribed user.
    :param session_id: Primary key of the session.
    :param subscription_service: Injected subscription service.
    :param current_user: Currently authenticated user.
    :return: The matching Subscription record.
    :raises HTTPException 403: If the current user is not the owner.
    :raises HTTPException 404: If no subscription exists for the given pair.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only access your own subscriptions")
    subscription = subscription_service.get_subscription(user_id, session_id)
    if subscription is None:
        raise HTTPException(status_code=404, detail="Subscription not found")
    return subscription

@subscription_router.post("/", response_model=SubscriptionResponse)
def create_subscription(request: SubscriptionCreationRequest, subscription_service: SubscriptionServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> Subscription:
    """Create a new subscription enrolling a user in a session.

    :param request: Request body containing user_id, session_id, and subscription_date.
    :param subscription_service: Injected subscription service.
    :param current_user: Currently authenticated user.
    :return: The newly created Subscription record.
    :raises HTTPException 403: If the current user tries to subscribe another user.
    """
    if current_user.id != request.user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only create subscriptions for yourself")
    return subscription_service.create_subscription(
        request.user_id,
        request.session_id,
        request.subscription_date
    )

@subscription_router.delete("/{user_id}/{session_id}", response_model=dict)
def delete_subscription(user_id: int, session_id: int, subscription_service: SubscriptionServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Delete a subscription, unenrolling a user from a session.

    :param user_id: Primary key of the subscribed user.
    :param session_id: Primary key of the session.
    :param subscription_service: Injected subscription service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    :raises HTTPException 403: If the current user is not the owner.
    :raises HTTPException 404: If no subscription exists for the given pair.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only delete your own subscriptions")
    success = subscription_service.delete_subscription(user_id, session_id)
    if success:
        return {"message": "Subscription deleted successfully"}
    else:
        raise HTTPException(status_code=404, detail="Subscription not found")
