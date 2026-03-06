from pydantic import BaseModel, ConfigDict, EmailStr
from typing import Optional

class UserCreationRequest(BaseModel):
    """
    Modèle de requête pour la création d'un utilisateur.
    Valide l'email, le mot de passe et l'adresse fournis par le client.
    """
    model_config = ConfigDict(extra="forbid")

    email: EmailStr  #: Adresse email de l'utilisateur
    password: str    #: Mot de passe en clair (sera hashé côté serveur)
    address: Optional[str] = None     #: Adresse postale de l'utilisateur