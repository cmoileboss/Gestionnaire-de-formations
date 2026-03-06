using NetCoreAPI.DTOs;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des recommandations IA.
/// </summary>
public interface IAirecommandationService
{
    /// <summary>Récupère toutes les recommandations IA.</summary>
    Task<IEnumerable<AirecommandationDto>> GetAllAsync();

    /// <summary>Récupère une recommandation IA par son identifiant.</summary>
    /// <param name="id">Identifiant de la recommandation.</param>
    Task<AirecommandationDto?> GetByIdAsync(int id);

    /// <summary>Crée une nouvelle recommandation IA.</summary>
    /// <param name="dto">DTO de la recommandation à créer.</param>
    Task<AirecommandationDto> CreateAsync(AirecommandationDto dto);

    /// <summary>Met à jour une recommandation IA existante.</summary>
    /// <param name="id">Identifiant de la recommandation.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<AirecommandationDto> UpdateAsync(int id, AirecommandationDto dto);

    /// <summary>Supprime une recommandation IA par son identifiant.</summary>
    /// <param name="id">Identifiant de la recommandation à supprimer.</param>
    Task<bool> DeleteAsync(int id);
}
