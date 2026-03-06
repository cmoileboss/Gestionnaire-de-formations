using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des résultats d'évaluation.
    /// Définit les opérations CRUD sur les résultats (clé composite UserId + EvaluationId).
    /// </summary>
    public interface IResultRepository
    {
        /// <summary>Récupère tous les résultats.</summary>
        Task<IEnumerable<Result>> GetAllAsync();

        /// <summary>Récupère un résultat par sa clé composite.</summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="evaluationId">Identifiant de l'évaluation.</param>
        Task<Result?> GetByIdAsync(int userId, int evaluationId);

        /// <summary>Ajoute un nouveau résultat.</summary>
        /// <param name="entity">Résultat à ajouter.</param>
        Task AddAsync(Result entity);

        /// <summary>Met à jour un résultat existant.</summary>
        /// <param name="entity">Résultat à mettre à jour.</param>
        Task UpdateAsync(Result entity);

        /// <summary>Supprime un résultat par sa clé composite.</summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="evaluationId">Identifiant de l'évaluation.</param>
        Task DeleteAsync(int userId, int evaluationId);
    }
}
