using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des abonnements.
    /// Définit les opérations CRUD sur les abonnements.
    /// </summary>
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// Récupère tous les abonnements.
        /// </summary>
        /// <returns>Liste des abonnements.</returns>
        Task<IEnumerable<Subscription>> GetAllAsync();

        /// <summary>
        /// Récupère un abonnement par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement.</param>
        /// <returns>L'abonnement ou null.</returns>
        Task<Subscription?> GetByIdAsync(int id);

        /// <summary>
        /// Ajoute un nouvel abonnement.
        /// </summary>
        /// <param name="entity">Abonnement à ajouter.</param>
        Task AddAsync(Subscription entity);

        /// <summary>
        /// Met à jour un abonnement existant.
        /// </summary>
        /// <param name="entity">Abonnement à mettre à jour.</param>
        Task UpdateAsync(Subscription entity);

        /// <summary>
        /// Supprime un abonnement par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement à supprimer.</param>
        Task DeleteAsync(int id);
    }
}
