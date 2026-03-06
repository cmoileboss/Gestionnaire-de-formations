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



@app.exception_handler(RequestValidationError)
async def validation_exception_handler(request: Request, exc: RequestValidationError):
    """Gestion des erreurs de validation des données"""
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content={
            "error": "Erreur de validation",
            "detail": exc.errors(),
            "body": exc.body
        }
    )


@app.exception_handler(IntegrityError)
async def integrity_exception_handler(request: Request, exc: IntegrityError):
    """Gestion des erreurs d'intégrité de la base de données (ex: violations de contraintes)"""
    return JSONResponse(
        status_code=status.HTTP_409_CONFLICT,
        content={
            "error": "Conflit de données",
            "detail": "Les données fournies violent une contrainte d'intégrité (ex: email déjà existant)"
        }
    )


@app.exception_handler(SQLAlchemyError)
async def sqlalchemy_exception_handler(request: Request, exc: SQLAlchemyError):
    """Gestion des erreurs SQLAlchemy génériques"""
    return JSONResponse(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        content={
            "error": "Erreur de base de données",
            "detail": str(exc)
        }
    )


@app.exception_handler(ValueError)
async def value_error_handler(request: Request, exc: ValueError):
    """Gestion des erreurs de valeur (données invalides)"""
    return JSONResponse(
        status_code=status.HTTP_400_BAD_REQUEST,
        content={
            "error": "Données invalides",
            "detail": str(exc)
        }
    )


@app.exception_handler(Exception)
async def general_exception_handler(request: Request, exc: Exception):
    """Gestion des erreurs non gérées (catch-all)"""
    return JSONResponse(
        status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
        content={
            "error": "Erreur interne du serveur",
            "detail": "Une erreur inattendue s'est produite"
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