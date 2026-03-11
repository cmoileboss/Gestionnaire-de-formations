using NetCoreAPI.DTOs;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des sessions.
/// </summary>
public interface ISessionService
{
    /// <summary>Récupère toutes les sessions.</summary>
    Task<Result<IEnumerable<SessionDto>>> GetAllAsync();

    /// <summary>Récupère une session par son identifiant.</summary>
    /// <param name="id">Identifiant de la session.</param>
    Task<Result<SessionDto>> GetByIdAsync(int id);

    /// <summary>Crée une nouvelle session.</summary>
    /// <param name="dto">DTO de la session à créer.</param>
    Task<Result<SessionDto>> CreateAsync(SessionDto dto);

    /// <summary>Met à jour une session existante.</summary>
    /// <param name="id">Identifiant de la session.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<Result<SessionDto>> UpdateAsync(int id, SessionDto dto);

    /// <summary>Supprime une session par son identifiant.</summary>
    /// <param name="id">Identifiant de la session à supprimer.</param>
    Task<Result<bool>> DeleteAsync(int id);

    /// <summary>Récupère tous les utilisateurs inscrits à une session.</summary>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result contenant la liste des utilisateurs ou un message d'erreur.</returns>
    Task<Result<IEnumerable<UserDto>>> GetSessionUsersAsync(int sessionId);
}
