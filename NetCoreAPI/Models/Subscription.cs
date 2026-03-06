
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente l'inscription d'un utilisateur à une session de formation.
/// Clé composite : identifiant utilisateur + identifiant session.
/// </summary>
[Table("Subscription")]
public partial class Subscription
{
    /// <summary>Identifiant de l'utilisateur inscrit (partie de la clé composite).</summary>
    [Key]
    [Column("user_id", Order = 0)]
    [Required(ErrorMessage = "L'utilisateur est obligatoire")]
    public int UserId { get; set; }

    /// <summary>Identifiant de la session visée (partie de la clé composite).</summary>
    [Key]
    [Column("session_id", Order = 1)]
    [Required(ErrorMessage = "La session est obligatoire")]
    public int SessionId { get; set; }

    /// <summary>Date à laquelle l'inscription a été effectuée.</summary>
    [Required(ErrorMessage = "La date d'abonnement est obligatoire")]
    [Column("subscription_date")]
    public DateTime SubscriptionDate { get; set; }

    /// <summary>Session à laquelle l'utilisateur est inscrit.</summary>
    public virtual Session Session { get; set; } = null!;

    /// <summary>Utilisateur qui s'est inscrit.</summary>
    public virtual User User { get; set; } = null!;
}
