using NetCoreAPI.DTOs;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des sessions.
/// </summary>
public interface ISessionService
{
    /// <summary>Récupère toutes les sessions.</summary>
    Task<IEnumerable<SessionDto>> GetAllAsync();

    /// <summary>Récupère une session par son identifiant.</summary>
    /// <param name="id">Identifiant de la session.</param>
    Task<SessionDto?> GetByIdAsync(int id);

    /// <summary>Crée une nouvelle session.</summary>
    /// <param name="dto">DTO de la session à créer.</param>
    Task<SessionDto> CreateAsync(SessionDto dto);

    /// <summary>Met à jour une session existante.</summary>
    /// <param name="id">Identifiant de la session.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<SessionDto> UpdateAsync(int id, SessionDto dto);

    /// <summary>Supprime une session par son identifiant.</summary>
    /// <param name="id">Identifiant de la session à supprimer.</param>
    Task<bool> DeleteAsync(int id);
}
