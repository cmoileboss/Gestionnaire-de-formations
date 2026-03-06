"""Router for user endpoints: CRUD operations, session subscriptions, and evaluation enrollments."""


from fastapi import APIRouter, Depends, HTTPException, Response
from sqlalchemy.orm import Session
from typing import Annotated

from services.UserService import UserService
from services.SecurityService import SecurityService

from database_connection import get_db

from models.User import User
from models.Session import Session as SessionModel
from models.Evaluation import Evaluation

from apirequests.UserCreationRequest import UserCreationRequest
from apirequests.LoginRegisterRequest import LoginRegisterRequest

from apiresponses.UserResponse import UserResponse
from apiresponses.SessionResponse import SessionResponse
from apiresponses.EvaluationResponse import EvaluationResponse

def get_user_service(db: Session = Depends(get_db)) -> UserService:
    """Get a UserService instance with the database session"""
    return UserService(db)

users_router = APIRouter(prefix="/users", tags=["Users"])

UserServiceDep = Annotated[UserService, Depends(get_user_service)]

@users_router.post("/login", response_model=UserResponse)
def login(response: Response, request: LoginRegisterRequest, user_service: UserServiceDep) -> User:
    """Endpoint de login utilisateur avec création de JWT."""
    try:
        user = user_service.login(request.email, request.password)
    except ValueError:
        raise HTTPException(status_code=401, detail="Email ou mot de passe invalide")
    token = SecurityService.create_access_token(user.email)
    response.set_cookie(key="access_token", value=token, httponly=True, secure=False, samesite="lax", max_age=3600)
    return user

@users_router.post("/logout")
def logout(response: Response):
    """Endpoint de logout utilisateur en supprimant le cookie d'accès."""
    response.delete_cookie("access_token")
    return {"message": "Déconnecté"}
    
@users_router.post("/register", response_model=UserResponse)
def register(request: LoginRegisterRequest, user_service: UserServiceDep) -> User:
    """Endpoint d'inscription utilisateur avec création de compte."""
    new_user = user_service.create_user(request.email, request.password)
    return new_user

@users_router.get("/", response_model=list[UserResponse])
def read_users(user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> list[User]:
    """Return the list of all registered users.

    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: List of all User records serialized as UserResponse.
    """
    users = user_service.get_all_users()
    return users

@users_router.get("/{user_id}", response_model=UserResponse|dict)
def read_user(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> User | dict:
    """Return a single user by their identifier.

    :param user_id: Primary key of the user to retrieve.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: The matching User record.
    :raises HTTPException 404: If no user exists with the given ID.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only access your own user information")
    user = user_service.get_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return user


@users_router.put("/{user_id}", response_model=UserResponse|dict)
def update_user(user_id: int, request: UserCreationRequest, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> User | dict:
    """Update an existing user's information.

    :param user_id: Primary key of the user to update.
    :param request: Request body with updated email, password, and/or address.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: The updated User record.
    :raises HTTPException 404: If no user exists with the given ID.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only update your own user information")
    updated_user = user_service.update_user(user_id, request.email, request.password, request.address)
    if updated_user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return updated_user

@users_router.delete("/{user_id}", response_model=dict)
def delete_user(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Delete a user by their identifier.

    :param user_id: Primary key of the user to delete.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    :raises HTTPException 404: If no user exists with the given ID.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only delete your own user account")
    success = user_service.delete_user(user_id)
    if success:
        return {"message": "User deleted successfully"}
    else:
        raise HTTPException(status_code=404, detail="User not found")

# Endpoints for managing user sessions (subscriptions)

@users_router.get("/{user_id}/sessions", response_model=list[SessionResponse])
def get_user_sessions(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> list[SessionModel]:
    """Return all training sessions a user is enrolled in via subscriptions.

    :param user_id: Primary key of the user.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: List of Session records the user is subscribed to.
    :raises HTTPException 404: If no user exists with the given ID.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only access your own session information")
    user = user_service.get_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return user_service.get_user_sessions(user_id)

@users_router.post("/{user_id}/sessions/{session_id}", response_model=dict)
def subscribe_user_to_session(user_id: int, session_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Enroll a user in a training session by creating a subscription.

    :param user_id: Primary key of the user to subscribe.
    :param session_id: Primary key of the session to subscribe the user to.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    :raises HTTPException 404: If the user or session does not exist.
    :raises HTTPException 400: If the user is already subscribed to the session.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only manage your own session subscriptions")
    user = user_service.get_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    try:
        result = user_service.subscribe_to_session(user_id, session_id)
    except ValueError:
        raise HTTPException(status_code=400, detail="User already subscribed to this session")
    if result is None:
        raise HTTPException(status_code=404, detail="Session not found")
    return {"message": "User subscribed to session successfully"}

@users_router.delete("/{user_id}/sessions/{session_id}", response_model=dict)
def unsubscribe_user_from_session(user_id: int, session_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Remove a user's subscription from a training session.

    :param user_id: Primary key of the user.
    :param session_id: Primary key of the session.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    :raises HTTPException 404: If the user or subscription does not exist.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only manage your own session subscriptions")
    user = user_service.get_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    success = user_service.unsubscribe_from_session(user_id, session_id)
    if success:
        return {"message": "User unsubscribed from session successfully"}
    else:
        raise HTTPException(status_code=404, detail="Subscription not found")

# Endpoints for managing user evaluations

@users_router.get("/{user_id}/evaluations", response_model=list[EvaluationResponse])
def get_user_evaluations(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> list[Evaluation]:
    """Return all evaluations a user is enrolled in.

    :param user_id: Primary key of the user.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: List of Evaluation records the user participates in.
    :raises HTTPException 404: If no user exists with the given ID.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only access your own evaluation information")
    user = user_service.get_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return user_service.get_user_evaluations(user_id)

@users_router.post("/{user_id}/evaluations/{evaluation_id}", response_model=dict)
def enroll_user_in_evaluation(user_id: int, evaluation_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Enroll a user in a specific evaluation.

    :param user_id: Primary key of the user to enroll.
    :param evaluation_id: Primary key of the target evaluation.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    :raises HTTPException 404: If the user or evaluation does not exist.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only manage your own evaluations")
    user = user_service.get_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    user_service.enroll_in_evaluation(user_id, evaluation_id)
    return {"message": "User enrolled in evaluation successfully"}


@users_router.delete("/{user_id}/evaluations/{evaluation_id}", response_model=dict)
def unenroll_user_from_evaluation(user_id: int, evaluation_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Remove a user's enrollment from an evaluation.

    :param user_id: Primary key of the user.
    :param evaluation_id: Primary key of the evaluation.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    :raises HTTPException 404: If the user or enrollment does not exist.
    """
    if current_user.id != user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only manage your own evaluations")
    user = user_service.get_user(user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    user_service.unenroll_from_evaluation(user_id, evaluation_id)
    return {"message": "User unenrolled from evaluation successfully"}

