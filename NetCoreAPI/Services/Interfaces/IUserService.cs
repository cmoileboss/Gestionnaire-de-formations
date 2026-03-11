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

    /// <summary>
    /// Récupère toutes les sessions auxquelles un utilisateur est inscrit.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Result contenant la liste des sessions ou un message d'erreur.</returns>
    Task<Result<IEnumerable<SessionDto>>> GetUserSessionsAsync(int userId);

    /// <summary>
    /// Inscrit un utilisateur à une session de formation.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result indiquant le succès ou l'échec de l'inscription.</returns>
    Task<Result<bool>> SubscribeToSessionAsync(int userId, int sessionId);

    /// <summary>
    /// Désinscrit un utilisateur d'une session de formation.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result indiquant le succès ou l'échec de la désinscription.</returns>
    Task<Result<bool>> UnsubscribeFromSessionAsync(int userId, int sessionId);

    /// <summary>
    /// Récupère toutes les évaluations auxquelles un utilisateur est inscrit.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Result contenant la liste des évaluations ou un message d'erreur.</returns>
    Task<Result<IEnumerable<EvaluationDto>>> GetUserEvaluationsAsync(int userId);

    /// <summary>
    /// Inscrit un utilisateur à une évaluation.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="evaluationId">Identifiant de l'évaluation.</param>
    /// <returns>Result indiquant le succès ou l'échec de l'inscription.</returns>
    Task<Result<bool>> EnrollInEvaluationAsync(int userId, int evaluationId);

    /// <summary>
    /// Désinscrit un utilisateur d'une évaluation.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="evaluationId">Identifiant de l'évaluation.</param>
    /// <returns>Result indiquant le succès ou l'échec de la désinscription.</returns>
    Task<Result<bool>> UnenrollFromEvaluationAsync(int userId, int evaluationId);
}