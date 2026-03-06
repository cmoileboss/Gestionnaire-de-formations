using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des évaluations.
    /// Définit les opérations CRUD sur les évaluations.
    /// </summary>
    public interface IEvaluationRepository
    {
        /// <summary>
        /// Récupère toutes les évaluations.
        /// </summary>
        /// <returns>Liste des évaluations.</returns>
        Task<IEnumerable<Evaluation>> GetAllAsync();

        /// <summary>
        /// Récupère une évaluation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'évaluation.</param>
        /// <returns>L'évaluation ou null.</returns>
        Task<Evaluation?> GetByIdAsync(int id);

        /// <summary>
        /// Ajoute une nouvelle évaluation.
        /// </summary>
        /// <param name="entity">Évaluation à ajouter.</param>
        Task AddAsync(Evaluation entity);

        /// <summary>
        /// Met à jour une évaluation existante.
        /// </summary>
        /// <param name="entity">Évaluation à mettre à jour.</param>
        Task UpdateAsync(Evaluation entity);

        /// <summary>
        /// Supprime une évaluation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'évaluation à supprimer.</param>
        Task DeleteAsync(int id);
    }
}
