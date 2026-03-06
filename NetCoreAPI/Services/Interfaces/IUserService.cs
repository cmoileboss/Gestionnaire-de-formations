using NetCoreAPI.DTOs;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des utilisateurs.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Récupère tous les utilisateurs.
    /// </summary>
    /// <returns>Liste des DTO utilisateurs.</returns>
    Task<IEnumerable<UserDto>> GetAllAsync();

    /// <summary>
    /// Récupère un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur.</param>
    /// <returns>Le DTO utilisateur ou null.</returns>
    Task<UserDto?> GetByIdAsync(int id);

    /// <summary>
    /// Récupère un utilisateur par son email.
    /// </summary>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <returns>Le DTO utilisateur ou null.</returns>
    Task<UserDto?> GetByEmailAsync(string email);

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="userDto">DTO utilisateur à créer.</param>
    /// <returns>Le DTO utilisateur créé.</returns>
    Task<UserDto> CreateAsync(UserCreationUpdateDto userDto);

    /// <summary>
    /// Met à jour un utilisateur existant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à mettre à jour.</param>
    /// <param name="userDto">DTO utilisateur à mettre à jour.</param>
    /// <returns>Le DTO utilisateur mis à jour.</returns>
    Task<UserDto> UpdateAsync(int id, UserCreationUpdateDto userDto);

    /// <summary>
    /// Supprime un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à supprimer.</param>
    /// <returns>True si supprimé, sinon false.</returns>
    Task<bool> DeleteAsync(int id);
}