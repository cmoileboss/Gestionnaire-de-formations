using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente un utilisateur de la plateforme de gestion des formations.
/// </summary>
[Table("Users")]
public partial class User
{
    /// <summary>Identifiant unique de l'utilisateur.</summary>
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>Hash du mot de passe de l'utilisateur (min. 8 caractères).</summary>
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [PasswordPropertyText(true)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit contenir entre 8 et 100 caractères")]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    /// <summary>Adresse email unique de l'utilisateur.</summary>
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    [StringLength(255, ErrorMessage = "L'email ne doit pas dépasser 255 caractères")]
    [Column("email")]
    public string Email { get; set; } = null!;

    /// <summary>Adresse postale de l'utilisateur (optionnelle).</summary>
    [StringLength(255, ErrorMessage = "L'adresse ne doit pas dépasser 255 caractères")]
    [Column("address")]
    public string? Address { get; set; }

    /// <summary>Résultats d'évaluation associés à l'utilisateur.</summary>
    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    /// <summary>Abonnements aux sessions de l'utilisateur.</summary>
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
