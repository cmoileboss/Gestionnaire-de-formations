using NetCoreAPI.DTOs;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des résultats d'évaluation.
/// </summary>
public interface IResultService
{
    /// <summary>Récupère tous les résultats.</summary>
    Task<IEnumerable<ResultDto>> GetAllAsync();

    /// <summary>Récupère un résultat par sa clé composite.</summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="evaluationId">Identifiant de l'évaluation.</param>
    Task<ResultDto?> GetByIdAsync(int userId, int evaluationId);

    /// <summary>Crée un nouveau résultat.</summary>
    /// <param name="dto">DTO du résultat à créer.</param>
    Task<ResultDto> CreateAsync(ResultDto dto);

    /// <summary>Met à jour un résultat existant.</summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="evaluationId">Identifiant de l'évaluation.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<ResultDto> UpdateAsync(int userId, int evaluationId, ResultDto dto);

    /// <summary>Supprime un résultat par sa clé composite.</summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="evaluationId">Identifiant de l'évaluation.</param>
    Task<bool> DeleteAsync(int userId, int evaluationId);
}
