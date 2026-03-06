from pydantic import BaseModel, ConfigDict

class ModuleResponse(BaseModel):
    """Pydantic response model for a training module.

    Returned by module endpoints.
    """

    model_config = ConfigDict(from_attributes=True)

    #: Unique identifier of the module.
    module_id: int
    #: Title of the module.
    title: str
    #: Subject or theme area of the module.
    subject: str
    #: Detailed description of the module content.
    description: str
