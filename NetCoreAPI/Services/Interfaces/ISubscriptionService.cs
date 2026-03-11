using NetCoreAPI.DTOs;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des abonnements.
/// </summary>
public interface ISubscriptionService
{
    /// <summary>Récupère tous les abonnements.</summary>
    Task<Result<IEnumerable<SubscriptionDto>>> GetAllAsync();

    /// <summary>Récupère un abonnement par son identifiant.</summary>
    /// <param name="id">Identifiant de l'abonnement.</param>
    Task<Result<SubscriptionDto>> GetByIdAsync(int id);

    /// <summary>Crée un nouvel abonnement.</summary>
    /// <param name="dto">DTO de l'abonnement à créer.</param>
    Task<Result<SubscriptionDto>> CreateAsync(SubscriptionDto dto);

    /// <summary>Met à jour un abonnement existant.</summary>
    /// <param name="id">Identifiant de l'abonnement.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<Result<SubscriptionDto>> UpdateAsync(int id, SubscriptionDto dto);

    /// <summary>Supprime un abonnement par son identifiant.</summary>
    /// <param name="id">Identifiant de l'abonnement à supprimer.</param>
    Task<Result<bool>> DeleteAsync(int id);

    /// <summary>Récupère tous les abonnements d'un utilisateur.</summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Result contenant la liste des abonnements ou un message d'erreur.</returns>
    Task<Result<IEnumerable<SubscriptionDto>>> GetByUserIdAsync(int userId);

    /// <summary>Récupère tous les abonnements d'une session.</summary>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result contenant la liste des abonnements ou un message d'erreur.</returns>
    Task<Result<IEnumerable<SubscriptionDto>>> GetBySessionIdAsync(int sessionId);

    /// <summary>Récupère un abonnement par utilisateur et session.</summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result contenant l'abonnement ou un message d'erreur.</returns>
    Task<Result<SubscriptionDto>> GetByUserAndSessionAsync(int userId, int sessionId);

    /// <summary>Supprime un abonnement par utilisateur et session.</summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result indiquant le succès ou l'échec de la suppression.</returns>
    Task<Result<bool>> DeleteByUserAndSessionAsync(int userId, int sessionId);
}
