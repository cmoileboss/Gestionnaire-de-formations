using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Interface du repository pour la gestion des utilisateurs.
    /// Définit les opérations CRUD et de recherche utilisateur.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Récupère tous les utilisateurs.
        /// </summary>
        /// <returns>Liste des utilisateurs.</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Récupère un utilisateur par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <returns>L'utilisateur ou null.</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Récupère un utilisateur par son email.
        /// </summary>
        /// <param name="email">Email de l'utilisateur.</param>
        /// <returns>L'utilisateur ou null.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Crée un nouvel utilisateur.
        /// </summary>
        /// <param name="user">Utilisateur à créer.</param>
        /// <returns>L'utilisateur créé.</returns>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// Met à jour un utilisateur existant.
        /// </summary>
        /// <param name="user">Utilisateur à mettre à jour.</param>
        /// <returns>L'utilisateur mis à jour.</returns>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// Supprime un utilisateur par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à supprimer.</param>
        /// <returns>True si supprimé, sinon false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Vérifie si un utilisateur existe par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <returns>True si l'utilisateur existe.</returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Vérifie si un email existe déjà.
        /// </summary>
        /// <param name="email">Email à vérifier.</param>
        /// <returns>True si l'email existe.</returns>
        Task<bool> EmailExistsAsync(string email);
    }
}