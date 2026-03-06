using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant l'inscription d'un utilisateur à une session de formation.
/// </summary>
public class SubscriptionDto
{
    /// <summary>Identifiant de l'utilisateur inscrit.</summary>
    [Required(ErrorMessage = "L'utilisateur est obligatoire")]
    public int UserId { get; set; }

    /// <summary>Identifiant de la session visée.</summary>
    [Required(ErrorMessage = "La session est obligatoire")]
    public int SessionId { get; set; }

    /// <summary>Date à laquelle l'inscription a été effectuée.</summary>
    [Required(ErrorMessage = "La date d'abonnement est obligatoire")]
    public DateTime SubscriptionDate { get; set; }
}