from pydantic import BaseModel, ConfigDict, EmailStr

from models.UserRolesEnum import UserRoles

class RegisterRequest(BaseModel):
    model_config = ConfigDict(extra="forbid")
    
    email: EmailStr
    password: str
    role: UserRoles = UserRoles.USER