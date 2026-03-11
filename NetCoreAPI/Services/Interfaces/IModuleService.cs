using NetCoreAPI.DTOs;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des modules.
/// </summary>
public interface IModuleService
{
    /// <summary>Récupère tous les modules.</summary>
    Task<Result<IEnumerable<ModuleDto>>> GetAllAsync();

    /// <summary>Récupère un module par son identifiant.</summary>
    /// <param name="id">Identifiant du module.</param>
    Task<Result<ModuleDto>> GetByIdAsync(int id);

    /// <summary>Crée un nouveau module.</summary>
    /// <param name="dto">DTO du module à créer.</param>
    Task<Result<ModuleDto>> CreateAsync(ModuleDto dto);

    /// <summary>Met à jour un module existant.</summary>
    /// <param name="id">Identifiant du module.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<Result<ModuleDto>> UpdateAsync(int id, ModuleDto dto);

    /// <summary>Supprime un module par son identifiant.</summary>
    /// <param name="id">Identifiant du module à supprimer.</param>
    Task<Result<bool>> DeleteAsync(int id);
}
