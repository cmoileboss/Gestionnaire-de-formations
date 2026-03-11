using NetCoreAPI.DTOs;
using NetCoreAPI.Utils;

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
    Task<Result<IEnumerable<UserDto>>> GetAllAsync();

    /// <summary>
    /// Récupère un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur.</param>
    /// <returns>Result contenant le DTO utilisateur ou un message d'erreur.</returns>
    Task<Result<UserDto>> GetByIdAsync(int id);

    /// <summary>
    /// Récupère un utilisateur par son email.
    /// </summary>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <returns>Result contenant le DTO utilisateur ou un message d'erreur.</returns>
    Task<Result<UserDto>> GetByEmailAsync(string email);

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="userDto">DTO utilisateur à créer.</param>
    /// <returns>Result contenant le DTO utilisateur créé ou un message d'erreur.</returns>
    Task<Result<UserDto>> CreateAsync(UserCreationUpdateDto userDto);

    /// <summary>
    /// Met à jour un utilisateur existant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à mettre à jour.</param>
    /// <param name="userDto">DTO utilisateur à mettre à jour.</param>
    /// <returns>Result contenant le DTO utilisateur mis à jour ou un message d'erreur.</returns>
    Task<Result<UserDto>> UpdateAsync(int id, UserCreationUpdateDto userDto);

    /// <summary>
    /// Supprime un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à supprimer.</param>
    /// <returns>Result indiquant le succès ou l'échec de la suppression.</returns>
    Task<Result<bool>> DeleteAsync(int id);
}