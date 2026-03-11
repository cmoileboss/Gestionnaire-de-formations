using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des formations.
    /// Définit les opérations CRUD sur les formations.
    /// </summary>
    public interface IFormationRepository
    {
        /// <summary>
        /// Récupère toutes les formations.
        /// </summary>
        /// <returns>Liste des formations.</returns>
        Task<IEnumerable<Formation>> GetAllAsync();

        /// <summary>
        /// Récupère une formation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <returns>La formation ou null.</returns>
        Task<Formation?> GetByIdAsync(int id);

        /// <summary>
        /// Ajoute une nouvelle formation.
        /// </summary>
        /// <param name="entity">Formation à ajouter.</param>
        Task AddAsync(Formation entity);

        /// <summary>
        /// Met à jour une formation existante.
        /// </summary>
        /// <param name="entity">Formation à mettre à jour.</param>
        Task UpdateAsync(Formation entity);

        /// <summary>
        /// Supprime une formation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation à supprimer.</param>
        Task DeleteAsync(int id);

        /// <summary> 
        /// Vérifie si une formation existe par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <returns>True si la formation existe, sinon false.</returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Récupère une formation par son identifiant avec ses modules.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <returns>La formation avec ses modules ou null.</returns>
        Task<Formation?> GetByIdWithModulesAsync(int id);
    }
}
