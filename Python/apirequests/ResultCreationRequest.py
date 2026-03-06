from datetime import datetime

from typing import Optional
from pydantic import BaseModel, ConfigDict

class ResultCreationRequest(BaseModel):
    """Request body for creating a new evaluation result."""
    model_config = ConfigDict(extra="forbid")

    user_id: int
    evaluation_id: int
    score: float
    success: bool
    date: Optional[datetime] = datetime.now()