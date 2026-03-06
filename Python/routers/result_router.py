from fastapi import APIRouter, Depends, HTTPException, Response
from sqlalchemy.orm import Session
from typing import Annotated

from database_connection import get_db

from models.User import User

from apirequests.ResultCreationRequest import ResultCreationRequest
from apiresponses.ResultResponse import ResultResponse

from services.ResultService import ResultService
from services.SecurityService import SecurityService



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
    :raises HTTPException 403: If the current user tries to create a result for another user.
    """
    if current_user.id != result_request.user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only create results for yourself")
    return result_service.create_result(result_request.user_id, result_request.evaluation_id, result_request.score, result_request.success, result_request.date)

@result_router.get("/{result_id}", response_model=ResultResponse)
def read_result(result_id: int, result_service: ResultServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> ResultResponse:
    """Return a single evaluation result by its identifier.

    :param result_id: Primary key of the result to retrieve.
    :param result_service: Injected result service.
    :param current_user: Currently authenticated user.
    :return: The matching Result record.
    :raises HTTPException 403: If the result does not belong to the current user.
    :raises HTTPException 404: If no result exists with the given ID.
    """
    result = result_service.get_result(result_id)
    if result is None:
        raise HTTPException(status_code=404, detail="Result not found")
    if current_user.id != result.user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only access your own results")
    return result

@result_router.delete("/{result_id}", response_model=dict)
def delete_result(result_id: int, result_service: ResultServiceDep, current_user: User = Depends(SecurityService.get_current_user)) -> dict:
    """Delete an evaluation result by its identifier.

    :param result_id: Primary key of the result to delete.
    :param result_service: Injected result service.
    :param current_user: Currently authenticated user.
    :return: Confirmation message on success.
    :raises HTTPException 403: If the result does not belong to the current user.
    :raises HTTPException 404: If no result exists with the given ID.
    """
    result = result_service.get_result(result_id)
    if result is None:
        raise HTTPException(status_code=404, detail="Result not found")
    if current_user.id != result.user_id:
        raise HTTPException(status_code=403, detail="Forbidden: You can only delete your own results")
    result_service.delete_result(result_id)
    return {"message": "Result deleted successfully"}