using NetCoreAPI.DTOs;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des formations.
/// </summary>
public interface IFormationService
{
    /// <summary>
    /// Récupère toutes les formations.
    /// </summary>
    /// <returns>Result contenant la liste des DTO formations.</returns>
    Task<Result<IEnumerable<FormationDto>>> GetAllAsync();

    /// <summary>
    /// Récupère une formation par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de la formation.</param>
    /// <returns>Result contenant le DTO formation ou un message d'erreur.</returns>
    Task<Result<FormationDto>> GetByIdAsync(int id);

    /// <summary>
    /// Crée une nouvelle formation.
    /// </summary>
    /// <param name="formationDto">DTO formation à créer.</param>
    /// <returns>Result contenant le DTO formation créé ou un message d'erreur.</returns>
    Task<Result<FormationDto>> CreateAsync(FormationCreationUpdateDto formationDto);

    /// <summary>
    /// Met à jour une formation existante.
    /// </summary>
    /// <param name="id">Identifiant de la formation à mettre à jour.</param>
    /// <param name="formationDto">DTO formation à mettre à jour.</param>
    /// <returns>Result contenant le DTO formation mis à jour ou un message d'erreur.</returns>
    Task<Result<FormationDto>> UpdateAsync(int id, FormationCreationUpdateDto formationDto);

    /// <summary>
    /// Supprime une formation par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de la formation à supprimer.</param>
    /// <returns>Result indiquant le succès ou l'échec de la suppression.</returns>
    Task<Result<bool>> DeleteAsync(int id);

    /// <summary>
    /// Récupère tous les modules associés à une formation.
    /// </summary>
    /// <param name="formationId">Identifiant de la formation.</param>
    /// <returns>Result contenant la liste des modules ou un message d'erreur.</returns>
    Task<Result<IEnumerable<ModuleDto>>> GetFormationModulesAsync(int formationId);

    /// <summary>
    /// Associe un module existant à une formation.
    /// </summary>
    /// <param name="formationId">Identifiant de la formation.</param>
    /// <param name="moduleId">Identifiant du module.</param>
    /// <returns>Result indiquant le succès ou l'échec de l'association.</returns>
    Task<Result<bool>> AddModuleToFormationAsync(int formationId, int moduleId);

    /// <summary>
    /// Retire un module d'une formation.
    /// </summary>
    /// <param name="formationId">Identifiant de la formation.</param>
    /// <param name="moduleId">Identifiant du module.</param>
    /// <returns>Result indiquant le succès ou l'échec du retrait.</returns>
    Task<Result<bool>> RemoveModuleFromFormationAsync(int formationId, int moduleId);
}