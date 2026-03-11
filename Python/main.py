from fastapi import FastAPI, Request, status
from fastapi.responses import JSONResponse
from fastapi.exceptions import RequestValidationError
from fastapi.openapi.utils import get_openapi
from sqlalchemy.exc import SQLAlchemyError, IntegrityError

from database_connection import Base, engine

from routers.users_router import users_router
from routers.formation_router import formation_router
from routers.session_router import sessions_router
from routers.module_router import module_router
from routers.evaluation_router import evaluation_router
from routers.subscription_router import subscription_router
from routers.result_router import result_router

from models.User import User
from models.Formation import Formation
from models.Module import Module
from models.Session import Session
from models.Subscription import Subscription
from models.Result import Result
from models.Evaluation import Evaluation
from models.AIRecommandation import AIRecommandation
from models.ModuleUsage import t_ModuleUsage

from exceptions import (
    NotFoundError,
    DuplicateError,
    UnauthorizedError,
    ForbiddenError,
    ValidationError
)
from error_handlers import (
    not_found_handler,
    duplicate_handler,
    unauthorized_handler,
    forbidden_handler,
    validation_handler,
    integrity_error_handler,
    sqlalchemy_error_handler,
    general_exception_handler
)

app = FastAPI(
    title="API de gestion de formations - Python",
    description="API pour gérer les formations",
    version="1.0.0"
)

@app.on_event("startup")
def startup_event():
    """Créer les tables de la base de données au démarrage"""
    # Base.metadata.drop_all(bind=engine)  # Supprime toutes les tables
    # Base.metadata.create_all(bind=engine)


# Enregistrement des handlers d'exceptions personnalisées
app.add_exception_handler(NotFoundError, not_found_handler)
app.add_exception_handler(DuplicateError, duplicate_handler)
app.add_exception_handler(UnauthorizedError, unauthorized_handler)
app.add_exception_handler(ForbiddenError, forbidden_handler)
app.add_exception_handler(ValidationError, validation_handler)

# Enregistrement des handlers d'exceptions de base de données
app.add_exception_handler(IntegrityError, integrity_error_handler)
app.add_exception_handler(SQLAlchemyError, sqlalchemy_error_handler)

# Handler catch-all pour toutes les autres exceptions
app.add_exception_handler(Exception, general_exception_handler)


@app.exception_handler(RequestValidationError)
async def validation_exception_handler(request: Request, exc: RequestValidationError):
    """Gestion des erreurs de validation des données de requête FastAPI"""
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content={
            "error": "Erreur de validation",
            "detail": exc.errors(),
            "body": exc.body
        }
    )


@app.get("/", tags=["Health"])
async def root():
    """Vérification de l'état de l'API"""
    return {
        "message": "API de gestion de formations",
        "version": "1.0.0"
    }

app.include_router(users_router)
app.include_router(formation_router)
app.include_router(sessions_router)
app.include_router(module_router)
app.include_router(evaluation_router)
app.include_router(subscription_router)
app.include_router(result_router)

def custom_openapi():
    if app.openapi_schema:
        return app.openapi_schema
    schema = get_openapi(
        title=app.title,
        version=app.version,
        description=app.description,
        routes=app.routes,
    )
    schema.setdefault("components", {}).setdefault("securitySchemes", {})["CookieAuth"] = {
        "type": "apiKey",
        "in": "cookie",
        "name": "access_token",
    }
    for path in schema.get("paths", {}).values():
        for operation in path.values():
            operation.setdefault("security", [{"CookieAuth": []}])
    app.openapi_schema = schema
    return schema

app.openapi = custom_openapi