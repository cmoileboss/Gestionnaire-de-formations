"""Router for evaluation endpoints: CRUD operations and enrolled-user retrieval."""

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import Annotated

from limiter_config import limiter

from database_connection import get_db

from models.Evaluation import Evaluation
from models.User import User

from services.EvaluationService import EvaluationService
from services.SecurityService import SecurityService

from apirequests.EvaluationCreationRequest import EvaluationCreationRequest
from apiresponses.EvaluationResponse import EvaluationResponse

from apiresponses.UserResponse import UserResponse


def get_evaluation_service(db: Session = Depends(get_db)) -> EvaluationService:
    """Get an EvaluationService instance with the database session"""
    return EvaluationService(db)

evaluation_router = APIRouter(
    prefix="/evaluations", 
    tags=["Evaluations"], 
    dependencies=[
        Depends(SecurityService.get_current_user),
        Depends(limiter.limit("100/minute"))
    ]
)
EvaluationServiceDep = Annotated[EvaluationService, Depends(get_evaluation_service)]

@evaluation_router.get("/", response_model=list[EvaluationResponse])
def read_evaluations(evaluation_service: EvaluationServiceDep) -> list[Evaluation]:
    """Return the list of all evaluations.

    :param evaluation_service: Injected evaluation service.
    :return: List of all Evaluation records.
    """
    return evaluation_service.get_all_evaluations()

@evaluation_router.get("/{evaluation_id}", response_model=EvaluationResponse)
def read_evaluation(evaluation_id: int, evaluation_service: EvaluationServiceDep) -> Evaluation:
    """Return a single evaluation by its identifier.

    :param evaluation_id: Primary key of the evaluation to retrieve.
    :param evaluation_service: Injected evaluation service.
    :return: The matching Evaluation record.
    """
    return evaluation_service.get_evaluation(evaluation_id)

@evaluation_router.post("/", response_model=EvaluationResponse)
def create_evaluation(request: EvaluationCreationRequest, evaluation_service: EvaluationServiceDep) -> Evaluation:
    """Create a new evaluation.

    :param request: Request body containing start/end dates, place, and module_id.
    :param evaluation_service: Injected evaluation service.
    :return: The newly created Evaluation record.
    """
    return evaluation_service.create_evaluation(
        request.start_date,
        request.end_date,
        request.place,
        request.module_id
    )

@evaluation_router.put("/{evaluation_id}", response_model=EvaluationResponse)
def update_evaluation(evaluation_id: int, request: EvaluationCreationRequest, evaluation_service: EvaluationServiceDep) -> Evaluation:
    """Update an existing evaluation's details.

    :param evaluation_id: Primary key of the evaluation to update.
    :param request: Request body with updated dates, place, and/or module_id.
    :param evaluation_service: Injected evaluation service.
    :return: The updated Evaluation record.
    """
    return evaluation_service.update_evaluation(
        evaluation_id,
        request.start_date,
        request.end_date,
        request.place,
        request.module_id
    )

@evaluation_router.delete("/{evaluation_id}", response_model=dict)
def delete_evaluation(evaluation_id: int, evaluation_service: EvaluationServiceDep) -> dict:
    """Delete an evaluation by its identifier.

    :param evaluation_id: Primary key of the evaluation to delete.
    :param evaluation_service: Injected evaluation service.
    :return: Confirmation message on success.
    """
    evaluation_service.delete_evaluation(evaluation_id)
    return {"message": "Évaluation supprimée avec succès"}


@evaluation_router.get("/{evaluation_id}/users", response_model=list[UserResponse])
def get_evaluation_users(evaluation_id: int, evaluation_service: EvaluationServiceDep) -> list[User]:
    """Return all users enrolled in a specific evaluation.

    :param evaluation_id: Primary key of the evaluation.
    :param evaluation_service: Injected evaluation service.
    :return: List of User records enrolled in the evaluation.
    """
    return evaluation_service.get_evaluation_users(evaluation_id)
