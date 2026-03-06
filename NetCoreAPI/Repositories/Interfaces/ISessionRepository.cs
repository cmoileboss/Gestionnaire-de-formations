using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des sessions.
    /// Définit les opérations CRUD sur les sessions de formation.
    /// </summary>
    public interface ISessionRepository
    {
        /// <summary>
        /// Récupère toutes les sessions.
        /// </summary>
        /// <returns>Liste des sessions.</returns>
        Task<IEnumerable<Session>> GetAllAsync();

        /// <summary>
        /// Récupère une session par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la session.</param>
        /// <returns>La session ou null.</returns>
        Task<Session?> GetByIdAsync(int id);

        /// <summary>
        /// Ajoute une nouvelle session.
        /// </summary>
        /// <param name="entity">Session à ajouter.</param>
        Task AddAsync(Session entity);

        /// <summary>
        /// Met à jour une session existante.
        /// </summary>
        /// <param name="entity">Session à mettre à jour.</param>
        Task UpdateAsync(Session entity);

        /// <summary>
        /// Supprime une session par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la session à supprimer.</param>
        Task DeleteAsync(int id);
    }
}
