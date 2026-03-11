"""Custom exceptions for business logic errors.

In Python, exceptions are the idiomatic way to handle both expected and unexpected errors.
These custom exceptions help distinguish between different types of business errors.
"""


class NotFoundError(Exception):
    """Raised when a requested resource does not exist."""
    
    def __init__(self, resource: str, identifier: int | str):
        self.resource = resource
        self.identifier = identifier
        super().__init__(f"{resource} avec l'ID {identifier} non trouvé(e)")


class DuplicateError(Exception):
    """Raised when trying to create a resource that already exists."""
    
    def __init__(self, message: str):
        super().__init__(message)


class UnauthorizedError(Exception):
    """Raised when authentication fails."""
    
    def __init__(self, message: str = "Email ou mot de passe invalide"):
        super().__init__(message)


class ForbiddenError(Exception):
    """Raised when user doesn't have permission to access a resource."""
    
    def __init__(self, message: str = "Accès interdit"):
        super().__init__(message)


class ValidationError(Exception):
    """Raised when input data fails business validation rules."""
    
    def __init__(self, message: str):
        super().__init__(message)
