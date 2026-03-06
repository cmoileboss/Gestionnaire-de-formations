using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des recommandations IA.
    /// Définit les opérations CRUD sur les recommandations IA.
    /// </summary>
    public interface IAirecommandationRepository
    {
        /// <summary>
        /// Récupère toutes les recommandations IA.
        /// </summary>
        /// <returns>Liste des recommandations IA.</returns>
        Task<IEnumerable<Airecommandation>> GetAllAsync();

        /// <summary>
        /// Récupère une recommandation IA par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recommandation.</param>
        /// <returns>La recommandation ou null.</returns>
        Task<Airecommandation?> GetByIdAsync(int id);

        /// <summary>
        /// Ajoute une nouvelle recommandation IA.
        /// </summary>
        /// <param name="entity">Recommandation à ajouter.</param>
        Task AddAsync(Airecommandation entity);

        /// <summary>
        /// Met à jour une recommandation IA existante.
        /// </summary>
        /// <param name="entity">Recommandation à mettre à jour.</param>
        Task UpdateAsync(Airecommandation entity);

        /// <summary>
        /// Supprime une recommandation IA par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recommandation à supprimer.</param>
        Task DeleteAsync(int id);
    }
}
