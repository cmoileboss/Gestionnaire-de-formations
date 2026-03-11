using NetCoreAPI.DTOs;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des évaluations.
/// </summary>
public interface IEvaluationService
{
    /// <summary>Récupère toutes les évaluations.</summary>
    Task<Result<IEnumerable<EvaluationDto>>> GetAllAsync();

    /// <summary>Récupère une évaluation par son identifiant.</summary>
    /// <param name="id">Identifiant de l'évaluation.</param>
    Task<Result<EvaluationDto>> GetByIdAsync(int id);

    /// <summary>Crée une nouvelle évaluation.</summary>
    /// <param name="dto">DTO de l'évaluation à créer.</param>
    Task<Result<EvaluationDto>> CreateAsync(EvaluationDto dto);

    /// <summary>Met à jour une évaluation existante.</summary>
    /// <param name="id">Identifiant de l'évaluation.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<Result<EvaluationDto>> UpdateAsync(int id, EvaluationDto dto);

    /// <summary>Supprime une évaluation par son identifiant.</summary>
    /// <param name="id">Identifiant de l'évaluation à supprimer.</param>
    Task<Result<bool>> DeleteAsync(int id);

    /// <summary>Récupère tous les utilisateurs inscrits à une évaluation.</summary>
    /// <param name="evaluationId">Identifiant de l'évaluation.</param>
    /// <returns>Result contenant la liste des utilisateurs ou un message d'erreur.</returns>
    Task<Result<IEnumerable<UserDto>>> GetEvaluationUsersAsync(int evaluationId);
}
