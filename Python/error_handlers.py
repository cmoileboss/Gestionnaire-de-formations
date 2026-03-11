"""Global exception handlers for FastAPI application.

These handlers convert custom business exceptions into appropriate HTTP responses.
This follows the Python philosophy of using exceptions for flow control (EAFP).
"""

from fastapi import Request, status
from fastapi.responses import JSONResponse
from sqlalchemy.exc import SQLAlchemyError, IntegrityError

from exceptions import (
    NotFoundError,
    DuplicateError,
    UnauthorizedError,
    ForbiddenError,
    ValidationError
)


async def not_found_handler(request: Request, exc: NotFoundError) -> JSONResponse:
    """Handle resource not found errors with 404 status."""
    return JSONResponse(
        status_code=status.HTTP_404_NOT_FOUND,
        content={"error": str(exc)}
    )


async def duplicate_handler(request: Request, exc: DuplicateError) -> JSONResponse:
    """Handle duplicate resource errors with 409 Conflict status."""
    return JSONResponse(
        status_code=status.HTTP_409_CONFLICT,
        content={"error": str(exc)}
    )


async def unauthorized_handler(request: Request, exc: UnauthorizedError) -> JSONResponse:
    """Handle authentication errors with 401 Unauthorized status."""
    return JSONResponse(
        status_code=status.HTTP_401_UNAUTHORIZED,
        content={"error": str(exc)}
    )


async def forbidden_handler(request: Request, exc: ForbiddenError) -> JSONResponse:
    """Handle authorization errors with 403 Forbidden status."""
    return JSONResponse(
        status_code=status.HTTP_403_FORBIDDEN,
        content={"error": str(exc)}
    )


async def validation_handler(request: Request, exc: ValidationError) -> JSONResponse:
    """Handle business validation errors with 400 Bad Request status."""
    return JSONResponse(
        status_code=status.HTTP_400_BAD_REQUEST,
        content={"error": str(exc)}
    )


async def integrity_error_handler(request: Request, exc: IntegrityError) -> JSONResponse:
    """Handle database integrity constraint violations."""
    return JSONResponse(
        status_code=status.HTTP_409_CONFLICT,
        content={"error": "Violation de contrainte d'intégrité de la base de données"}
    )


async def sqlalchemy_error_handler(request: Request, exc: SQLAlchemyError) -> JSONResponse:
    """Handle general database errors with 500 Internal Server Error."""
    return JSONResponse(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        content={"error": "Une erreur de base de données s'est produite"}
    )


async def general_exception_handler(request: Request, exc: Exception) -> JSONResponse:
    """Catch-all handler for unexpected exceptions."""
    return JSONResponse(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        content={"error": "Une erreur interne s'est produite"}
    )
