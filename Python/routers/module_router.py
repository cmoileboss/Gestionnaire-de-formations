"""Router for module endpoints: CRUD operations on training modules."""

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import Annotated


from database_connection import get_db

from models.Module import Module

from services.ModuleService import ModuleService
from services.SecurityService import SecurityService

from apirequests.ModuleCreationRequest import ModuleCreationRequest
from apiresponses.ModuleResponse import ModuleResponse


def get_module_service(db: Session = Depends(get_db)) -> ModuleService:
    """Get a ModuleService instance with the database session"""
    return ModuleService(db)

module_router = APIRouter(
    prefix="/modules", 
    tags=["Modules"], 
    dependencies=[
        Depends(SecurityService.get_current_user),
    ]
)
ModuleServiceDep = Annotated[ModuleService, Depends(get_module_service)]

@module_router.get("/", response_model=list[ModuleResponse])
def read_modules(module_service: ModuleServiceDep) -> list[Module]:
    """Return the list of all training modules.

    :param module_service: Injected module service.
    :return: List of all Module records.
    """
    return module_service.get_all_modules()

@module_router.get("/{module_id}", response_model=ModuleResponse)
def read_module(module_id: int, module_service: ModuleServiceDep) -> Module:
    """Return a single training module by its identifier.

    :param module_id: Primary key of the module to retrieve.
    :param module_service: Injected module service.
    :return: The matching Module record.
    """
    return module_service.get_module(module_id)

@module_router.post("/", response_model=ModuleResponse)
def create_module(request: ModuleCreationRequest, module_service: ModuleServiceDep) -> Module:
    """Create a new training module.

    :param request: Request body containing title, subject, and description.
    :param module_service: Injected module service.
    :return: The newly created Module record.
    """
    return module_service.create_module(request.title, request.subject, request.description)

@module_router.put("/{module_id}", response_model=ModuleResponse)
def update_module(module_id: int, request: ModuleCreationRequest, module_service: ModuleServiceDep) -> Module:
    """Update an existing module's details.

    :param module_id: Primary key of the module to update.
    :param request: Request body with updated title, subject, and/or description.
    :param module_service: Injected module service.
    :return: The updated Module record.
    """
    return module_service.update_module(module_id, request.title, request.subject, request.description)

@module_router.delete("/{module_id}", response_model=dict)
def delete_module(module_id: int, module_service: ModuleServiceDep) -> dict:
    """Delete a training module by its identifier.

    :param module_id: Primary key of the module to delete.
    :param module_service: Injected module service.
    :return: Confirmation message on success.
    """
    module_service.delete_module(module_id)
    return {"message": "Module supprimé avec succès"}
