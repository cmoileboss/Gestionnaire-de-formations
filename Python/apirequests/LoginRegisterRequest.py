from pydantic import BaseModel, ConfigDict, EmailStr

class LoginRegisterRequest(BaseModel):
    model_config = ConfigDict(extra="forbid")
    
    email: EmailStr
    password: str