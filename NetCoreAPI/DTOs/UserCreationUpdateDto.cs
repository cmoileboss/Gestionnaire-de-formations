using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO utilisé pour la création ou la mise à jour d'un utilisateur.
/// </summary>
public class UserCreationUpdateDto
{
    /// <summary>Hash du mot de passe (8 à 100 caractères).</summary>
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [PasswordPropertyText(true)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit contenir entre 8 et 100 caractères")]
    public string PasswordHash { get; set; } = null!;

    /// <summary>Adresse email unique de l'utilisateur.</summary>
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    [StringLength(255, ErrorMessage = "L'email ne doit pas dépasser 255 caractères")]
    public string Email { get; set; } = null!;

    /// <summary>Adresse postale de l'utilisateur (optionnelle).</summary>
    [StringLength(255, ErrorMessage = "L'adresse ne doit pas dépasser 255 caractères")]
    public string? Address { get; set; } = null!;
}