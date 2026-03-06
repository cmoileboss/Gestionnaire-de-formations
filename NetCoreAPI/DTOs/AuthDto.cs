using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO pour l'authentification des utilisateurs.
/// Contient les propriétés nécessaires pour la connexion et l'inscription.
/// </summary>
public class AuthDto
{
    /// <summary>
    /// Nom d'utilisateur de l'utilisateur.
    /// </summary>
    [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire")]
    [StringLength(255, ErrorMessage = "Le nom d'utilisateur ne doit pas dépasser 255 caractères")]
    [EmailAddress(ErrorMessage = "Il faut indiquer une adresse email valide")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Mot de passe de l'utilisateur.
    /// </summary>
    public string Password { get; set; } = null!;
}