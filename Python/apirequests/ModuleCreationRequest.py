from pydantic import BaseModel, ConfigDict

class ModuleCreationRequest(BaseModel):
    """Request body for creating or updating a training module.

    :param title: Title of the module.
    :param subject: Subject or theme area of the module.
    :param description: Detailed description of the module content.
    """

    model_config = ConfigDict(extra="forbid")

    #: Title of the module.
    title: str
    #: Subject or theme area of the module.
    subject: str
    #: Detailed description of the module content.
    description: str