"""Router for session endpoints: CRUD operations and enrolled-user retrieval."""

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import Annotated


from database_connection import get_db

from models.Session import Session as SessionModel
from models.User import User

from services.SessionService import SessionService
from services.SecurityService import SecurityService

from apirequests.SessionCreationRequest import SessionCreationRequest
from apiresponses.SessionResponse import SessionResponse
from apiresponses.UserResponse import UserResponse


def get_session_service(db: Session = Depends(get_db)) -> SessionService:
    """Get a SessionService instance with the database session"""
    return SessionService(db)

sessions_router = APIRouter(
    prefix="/sessions", 
    tags=["Sessions"], 
    dependencies=[
        Depends(SecurityService.get_current_user),
    ]
)
SessionServiceDep = Annotated[SessionService, Depends(get_session_service)]

@sessions_router.get("/", response_model=list[SessionResponse])
def read_sessions(session_service: SessionServiceDep) -> list[SessionModel]:
    """Return the list of all training sessions.

    :param session_service: Injected session service.
    :return: List of all Session records.
    """
    return session_service.get_all_sessions()

@sessions_router.get("/{session_id}", response_model=SessionResponse)
def read_session(session_id: int, session_service: SessionServiceDep) -> SessionModel:
    """Return a single training session by its identifier.

    :param session_id: Primary key of the session to retrieve.
    :param session_service: Injected session service.
    :return: The matching Session record.
    """
    return session_service.get_session(session_id)

@sessions_router.post("/", response_model=SessionResponse)
def create_session(request: SessionCreationRequest, session_service: SessionServiceDep) -> SessionModel:
    """Create a new training session.

    :param request: Request body containing formation_id, dates, and place.
    :param session_service: Injected session service.
    :return: The newly created Session record.
    """
    return session_service.create_session(
        request.formation_id,
        request.start_date,
        request.end_date,
        request.place
    )

@sessions_router.put("/{session_id}", response_model=SessionResponse)
def update_session(session_id: int, request: SessionCreationRequest, session_service: SessionServiceDep) -> SessionModel:
    """Update an existing session's details.

    :param session_id: Primary key of the session to update.
    :param request: Request body with updated session data.
    :param session_service: Injected session service.
    :return: The updated Session record.
    """
    return session_service.update_session(
        session_id,
        request.formation_id,
        request.start_date,
        request.end_date,
        request.place
    )

@sessions_router.delete("/{session_id}", response_model=dict)
def delete_session(session_id: int, session_service: SessionServiceDep) -> dict:
    """Delete a training session by its identifier.

    :param session_id: Primary key of the session to delete.
    :param session_service: Injected session service.
    :return: Confirmation message on success.
    """
    session_service.delete_session(session_id)
    return {"message": "Session supprimée avec succès"}

# Endpoints for managing session users (subscriptions)

@sessions_router.get("/{session_id}/users", response_model=list[UserResponse])
def get_session_users(session_id: int, session_service: SessionServiceDep) -> list[User]:
    """Return all users enrolled in a given training session.

    :param session_id: Primary key of the session.
    :param session_service: Injected session service.
    :return: List of User records subscribed to the session.
    """
    return session_service.get_session_users(session_id)
