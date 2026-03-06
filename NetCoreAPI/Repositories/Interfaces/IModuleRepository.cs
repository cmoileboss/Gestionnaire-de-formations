using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des modules.
    /// Définit les opérations CRUD sur les modules de formation.
    /// </summary>
    public interface IModuleRepository
    {
        /// <summary>
        /// Récupère tous les modules.
        /// </summary>
        /// <returns>Liste des modules.</returns>
        Task<IEnumerable<Module>> GetAllAsync();

        /// <summary>
        /// Récupère un module par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du module.</param>
        /// <returns>Le module ou null.</returns>
        Task<Module?> GetByIdAsync(int id);

        /// <summary>
        /// Ajoute un nouveau module.
        /// </summary>
        /// <param name="entity">Module à ajouter.</param>
        Task AddAsync(Module entity);

        /// <summary>
        /// Met à jour un module existant.
        /// </summary>
        /// <param name="entity">Module à mettre à jour.</param>
        Task UpdateAsync(Module entity);

        /// <summary>
        /// Supprime un module par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du module à supprimer.</param>
        Task DeleteAsync(int id);
    }
}
