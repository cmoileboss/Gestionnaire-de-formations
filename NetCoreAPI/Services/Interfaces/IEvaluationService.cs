using NetCoreAPI.DTOs;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des évaluations.
/// </summary>
public interface IEvaluationService
{
    /// <summary>Récupère toutes les évaluations.</summary>
    Task<IEnumerable<EvaluationDto>> GetAllAsync();

    /// <summary>Récupère une évaluation par son identifiant.</summary>
    /// <param name="id">Identifiant de l'évaluation.</param>
    Task<EvaluationDto?> GetByIdAsync(int id);

    /// <summary>Crée une nouvelle évaluation.</summary>
    /// <param name="dto">DTO de l'évaluation à créer.</param>
    Task<EvaluationDto> CreateAsync(EvaluationDto dto);

    /// <summary>Met à jour une évaluation existante.</summary>
    /// <param name="id">Identifiant de l'évaluation.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<EvaluationDto> UpdateAsync(int id, EvaluationDto dto);

    /// <summary>Supprime une évaluation par son identifiant.</summary>
    /// <param name="id">Identifiant de l'évaluation à supprimer.</param>
    Task<bool> DeleteAsync(int id);
}
