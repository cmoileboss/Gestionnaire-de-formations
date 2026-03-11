from fastapi import APIRouter, Depends, Response
from sqlalchemy.orm import Session
from typing import Annotated

from database_connection import get_db

from models.User import User

from apirequests.ResultCreationRequest import ResultCreationRequest
from apiresponses.ResultResponse import ResultResponse

from services.ResultService import ResultService
from services.SecurityService import SecurityService

from exceptions import ForbiddenError



def get_result_service(db: Session = Depends(get_db)) -> ResultService:
    """Get a ResultService instance with the database session"""
    return ResultService(db)

result_router = APIRouter(prefix="/results", tags=["Results"], dependencies=[Depends(SecurityService.get_current_user)])
ResultServiceDep = Annotated[ResultService, Depends(get_result_service)]

@result_router.get("/", response_model=list[ResultResponse])
def read_results(result_service: ResultServiceDep) -> list[ResultResponse]:
    """Return the list of all evaluation results.

    :param result_service: Injected result service.
    :return: List of all Result records.
    """
    return result_service.get_all_results()

@result_router.post("/", response_model=ResultResponse)
def create_result(result_service: ResultServiceDep, result_request: ResultCreationRequest, current_user: User = Depends(SecurityService.get_current_user)) -> ResultResponse:
    """Create a new evaluation result.

    :param result_request: Request body containing the result details.
    :param result_service: Injected result service.
    :param current_user: Currently authenticated user.
    :return: The newly created Result record.
    """
    if current_user.id != result_request.user_id:
        raise ForbiddenError("Vous ne pouvez créer des résultats que pour vous-même")
    return result_service.create_result(result_request.user_id, result_request.evaluation_id, result_request.score, result_request.success, result_request.date)

@result_router.get("/{user_id}/{evaluation_id}", response_model=ResultResponse)
def read_result(user_id: int, evaluation_id: int, result_service: ResultServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> ResultResponse:
    """Return a single evaluation result by its composite key.

    :param user_id: User identifier (part of composite key).
    :param evaluation_id: Evaluation identifier (part of composite key).
    :param result_service: Injected result service.
    :param current_user: Currently authenticated user.
    :return: The matching Result record.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez accéder qu'à vos propres résultats")
    result = result_service.get_result(user_id, evaluation_id)
    return result

@result_router.delete("/{user_id}/{evaluation_id}", response_model=dict)
def delete_result(user_id: int, evaluation_id: int, result_service: ResultServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Delete an evaluation result by its composite key.

    :param user_id: User identifier (part of composite key).
    :param evaluation_id: Evaluation identifier (part of composite key).
    :param result_service: Injected result service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    """
    if current_user.id != user_id:
        raise ForbiddenError("Vous ne pouvez supprimer que vos propres résultats")
    result_service.delete_result(user_id, evaluation_id)
    return {"message": "Résultat supprimé avec succès"}