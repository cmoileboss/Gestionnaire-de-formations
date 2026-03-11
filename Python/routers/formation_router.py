"""Router for formation endpoints: CRUD operations and module assignment management."""

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import Annotated


from database_connection import get_db

from models.Formation import Formation
from models.Module import Module

from services.FormationService import FormationService
from services.SecurityService import SecurityService

from apirequests.FormationCreationRequest import FormationCreationRequest

from apiresponses.FormationResponse import FormationResponse
from apiresponses.ModuleResponse import ModuleResponse


def get_formation_service(db: Session = Depends(get_db)) -> FormationService:
    """Get a FormationService instance with the database session"""
    return FormationService(db)

formation_router = APIRouter(
    prefix="/formations", 
    tags=["Formations"], 
    dependencies=[
        Depends(SecurityService.get_current_user),
    ]
)
FormationServiceDep = Annotated[FormationService, Depends(get_formation_service)]

@formation_router.get("/", response_model=list[FormationResponse])
def read_formations(formation_service: FormationServiceDep) -> list[Formation]:
    """Return the list of all available formations.

    :param formation_service: Injected formation service.
    :return: List of all Formation records.
    """
    return formation_service.get_all_formations()

@formation_router.get("/{formation_id}", response_model=FormationResponse)
def read_formation(formation_id: int, formation_service: FormationServiceDep) -> Formation:
    """Return a single formation by its identifier.

    :param formation_id: Primary key of the formation to retrieve.
    :param formation_service: Injected formation service.
    :return: The matching Formation record.
    """
    return formation_service.get_formation(formation_id)

@formation_router.post("/", response_model=FormationResponse)
def create_formation(request: FormationCreationRequest, formation_service: FormationServiceDep) -> Formation:
    """Create a new formation.

    :param request: Request body containing the formation name and description.
    :param formation_service: Injected formation service.
    :return: The newly created Formation record.
    """
    return formation_service.create_formation(request.name, request.description)

@formation_router.put("/{formation_id}", response_model=FormationResponse)
def update_formation(formation_id: int, request: FormationCreationRequest, formation_service: FormationServiceDep) -> Formation:
    """Update an existing formation's details.

    :param formation_id: Primary key of the formation to update.
    :param request: Request body with updated name and/or description.
    :param formation_service: Injected formation service.
    :return: The updated Formation record.
    """
    return formation_service.update_formation(formation_id, request.name, request.description)

@formation_router.delete("/{formation_id}", response_model=dict)
def delete_formation(formation_id: int, formation_service: FormationServiceDep) -> dict:
    """Delete a formation by its identifier.

    :param formation_id: Primary key of the formation to delete.
    :param formation_service: Injected formation service.
    :return: Confirmation message on success.
    """
    formation_service.delete_formation(formation_id)
    return {"message": "Formation supprimée avec succès"}

# Endpoints for managing modules in formations

@formation_router.get("/{formation_id}/modules", response_model=list[ModuleResponse])
def get_formation_modules(formation_id: int, formation_service: FormationServiceDep) -> list[Module]:
    """Return all modules linked to a specific formation.

    :param formation_id: Primary key of the formation.
    :param formation_service: Injected formation service.
    :return: List of Module records associated with the formation.
    """
    return formation_service.get_formation_modules(formation_id)

@formation_router.post("/{formation_id}/modules/{module_id}", response_model=dict)
def add_module_to_formation(formation_id: int, module_id: int, formation_service: FormationServiceDep) -> dict:
    """Link an existing module to a formation.

    :param formation_id: Primary key of the target formation.
    :param module_id: Primary key of the module to add.
    :param formation_service: Injected formation service.
    :return: Confirmation message on success.
    """
    formation_service.add_module_to_formation(formation_id, module_id)
    return {"message": "Module ajouté à la formation avec succès"}

@formation_router.delete("/{formation_id}/modules/{module_id}", response_model=dict)
def remove_module_from_formation(formation_id: int, module_id: int, formation_service: FormationServiceDep) -> dict:
    """Unlink a module from a formation.

    :param formation_id: Primary key of the formation.
    :param module_id: Primary key of the module to remove.
    :param formation_service: Injected formation service.
    :return: Confirmation message on success.
    """
    formation_service.remove_module_from_formation(formation_id, module_id)
    return {"message": "Module retiré de la formation avec succès"}
