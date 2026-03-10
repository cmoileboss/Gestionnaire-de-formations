from pydantic import BaseModel, ConfigDict

class LDAPRequest(BaseModel):
    """
    Modèle de requête pour l'authentification au serveur LDAP.
    Contient le nom et le mot de passe de l'utilisateur à authentifier.
    """
    model_config = ConfigDict(extra="forbid")

    username: str         #: Nom de l'utilisateur
    password: str         #: Mot de passe de l'utilisateur
