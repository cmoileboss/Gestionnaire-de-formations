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

        /// <summary>
        /// Récupère tous les abonnements d'un utilisateur spécifique.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <returns>Liste des abonnements de l'utilisateur.</returns>
        Task<IEnumerable<Subscription>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Récupère tous les abonnements pour une session spécifique.
        /// </summary>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>Liste des abonnements de la session.</returns>
        Task<IEnumerable<Subscription>> GetBySessionIdAsync(int sessionId);

        /// <summary>
        /// Récupère un abonnement spécifique par utilisateur et session.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>L'abonnement ou null.</returns>
        Task<Subscription?> GetByUserAndSessionAsync(int userId, int sessionId);

        /// <summary>
        /// Supprime un abonnement par sa clé composite (utilisateur + session).
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        Task DeleteByCompositeKeyAsync(int userId, int sessionId);
    }
}
