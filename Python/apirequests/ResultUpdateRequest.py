from datetime import datetime

from typing import Optional
from pydantic import BaseModel, ConfigDict

class ResultUpdateRequest(BaseModel):
    """Request body for updating an existing evaluation result."""
    model_config = ConfigDict(extra="forbid")

    score: Optional[float] = None
    success: Optional[bool] = None
    date: Optional[datetime] = None
