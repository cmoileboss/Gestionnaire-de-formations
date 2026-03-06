using NetCoreAPI.DTOs;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des abonnements.
/// </summary>
public interface ISubscriptionService
{
    /// <summary>Récupère tous les abonnements.</summary>
    Task<IEnumerable<SubscriptionDto>> GetAllAsync();

    /// <summary>Récupère un abonnement par son identifiant.</summary>
    /// <param name="id">Identifiant de l'abonnement.</param>
    Task<SubscriptionDto?> GetByIdAsync(int id);

    /// <summary>Crée un nouvel abonnement.</summary>
    /// <param name="dto">DTO de l'abonnement à créer.</param>
    Task<SubscriptionDto> CreateAsync(SubscriptionDto dto);

    /// <summary>Met à jour un abonnement existant.</summary>
    /// <param name="id">Identifiant de l'abonnement.</param>
    /// <param name="dto">DTO avec les nouvelles données.</param>
    Task<SubscriptionDto> UpdateAsync(int id, SubscriptionDto dto);

    /// <summary>Supprime un abonnement par son identifiant.</summary>
    /// <param name="id">Identifiant de l'abonnement à supprimer.</param>
    Task<bool> DeleteAsync(int id);
}
