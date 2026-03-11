"""Router for user endpoints: CRUD operations, session subscriptions, and evaluation enrollments."""


from fastapi import APIRouter, Depends, Response
from sqlalchemy.orm import Session
from typing import Annotated

from services.UserService import UserService
from services.SecurityService import SecurityService
from services.LDAPService import LDAPService

from database_connection import get_db

from models.User import User
from models.Session import Session as SessionModel
from models.Evaluation import Evaluation

from apirequests.UserCreationRequest import UserCreationRequest
from apirequests.LoginRegisterRequest import LoginRegisterRequest
from apirequests.LDAPRequest import LDAPRequest

from apiresponses.UserResponse import UserResponse
from apiresponses.SessionResponse import SessionResponse
from apiresponses.EvaluationResponse import EvaluationResponse

from exceptions import ForbiddenError


def get_user_service(db: Session = Depends(get_db)) -> UserService:
    """Get a UserService instance with the database session"""
    return UserService(db)

def get_ldap_service() -> LDAPService:
    """Get a UserService instance with the .env values"""
    return LDAPService(389)  # Port 389 pour LDAP sans SSL, port 636 pour LDAPS avec SSL

users_router = APIRouter(prefix="/users", tags=["Users"])

UserServiceDep = Annotated[UserService, Depends(get_user_service)]
LDAPServiceDep = Annotated[LDAPService, Depends(get_ldap_service)]

@users_router.post("/login", response_model=UserResponse)
def login(response: Response, request: LoginRegisterRequest, user_service: UserServiceDep) -> User:
    """Endpoint de login utilisateur avec création de JWT. Lève UnauthorizedError si identifiants invalides."""
    user = user_service.login(request.email, request.password)
    token = SecurityService.create_access_token(user.email)
    response.set_cookie(
        key="access_token",
        value=token,
        httponly=True,
        secure=True,
        samesite="lax",
        max_age=3600)
    return user

@users_router.post("/ldap")
def ldap_login(request: LDAPRequest, ldap_service: LDAPServiceDep):
    is_authenticated = ldap_service.authenticate(request.username, request.password)
    if is_authenticated:
        return { "message": "Bonjour Mon Seigneur Patrice !" }
    return { "message": "Tentative d'usurpation repoussée" }

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

@users_router.get("/{user_id}", response_model=UserResponse)
def read_user(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> User:
    """Return a single user by their identifier.

    :param user_id: Primary key of the user to retrieve.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: The matching User record.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez accéder qu'à vos propres informations")
    return user_service.get_user(user_id)


@users_router.put("/{user_id}", response_model=UserResponse)
def update_user(user_id: int, request: UserCreationRequest, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> User:
    """Update an existing user's information.

    :param user_id: Primary key of the user to update.
    :param request: Request body with updated email, password, and/or address.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: The updated User record.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez modifier que vos propres informations")
    return user_service.update_user(user_id, request.email, request.password, request.address)

@users_router.delete("/{user_id}", response_model=dict)
def delete_user(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Delete a user by their identifier.

    :param user_id: Primary key of the user to delete.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez supprimer que votre propre compte")
    user_service.delete_user(user_id)
    return {"message": "Utilisateur supprimé avec succès"}

# Endpoints for managing user sessions (subscriptions)

@users_router.get("/{user_id}/sessions", response_model=list[SessionResponse])
def get_user_sessions(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> list[SessionModel]:
    """Return all training sessions a user is enrolled in via subscriptions.

    :param user_id: Primary key of the user.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: List of Session records the user is subscribed to.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez accéder qu'à vos propres informations de session")
    return user_service.get_user_sessions(user_id)

@users_router.post("/{user_id}/sessions/{session_id}", response_model=dict)
def subscribe_user_to_session(user_id: int, session_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Enroll a user in a training session by creating a subscription.

    :param user_id: Primary key of the user to subscribe.
    :param session_id: Primary key of the session to subscribe the user to.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez gérer que vos propres inscriptions")
    user_service.subscribe_to_session(user_id, session_id)
    return {"message": "Utilisateur inscrit à la session avec succès"}

@users_router.delete("/{user_id}/sessions/{session_id}", response_model=dict)
def unsubscribe_user_from_session(user_id: int, session_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Remove a user's subscription from a training session.

    :param user_id: Primary key of the user.
    :param session_id: Primary key of the session.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez gérer que vos propres inscriptions")
    user_service.unsubscribe_from_session(user_id, session_id)
    return {"message": "Utilisateur désinscrit de la session avec succès"}

# Endpoints for managing user evaluations

@users_router.get("/{user_id}/evaluations", response_model=list[EvaluationResponse])
def get_user_evaluations(user_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> list[Evaluation]:
    """Return all evaluations a user is enrolled in.

    :param user_id: Primary key of the user.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: List of Evaluation records the user participates in.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez accéder qu'à vos propres informations d'évaluation")
    return user_service.get_user_evaluations(user_id)

@users_router.post("/{user_id}/evaluations/{evaluation_id}", response_model=dict)
def enroll_user_in_evaluation(user_id: int, evaluation_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Enroll a user in a specific evaluation.

    :param user_id: Primary key of the user to enroll.
    :param evaluation_id: Primary key of the target evaluation.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez gérer que vos propres évaluations")
    user_service.enroll_in_evaluation(user_id, evaluation_id)
    return {"message": "Utilisateur inscrit à l'évaluation avec succès"}


@users_router.delete("/{user_id}/evaluations/{evaluation_id}", response_model=dict)
def unenroll_user_from_evaluation(user_id: int, evaluation_id: int, user_service: UserServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Remove a user's enrollment from an evaluation.

    :param user_id: Primary key of the user.
    :param evaluation_id: Primary key of the evaluation.
    :param user_service: Injected user service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez gérer que vos propres évaluations")
    user_service.unenroll_from_evaluation(user_id, evaluation_id)
    return {"message": "Utilisateur désinscrit de l'évaluation avec succès"}

