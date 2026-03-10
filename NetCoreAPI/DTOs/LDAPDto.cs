using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO pour l'authentification des utilisateurs sur le serveur LDAP.
/// Contient les propriétés nécessaires pour la connexion
/// </summary>
public class LDAPDto
{
    /// <summary>
    /// Nom d'utilisateur de l'utilisateur.
    /// </summary>
    [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire")]
    public string Username { get; set; } = null!;

    /// <summary>
    /// Mot de passe de l'utilisateur.
    /// </summary>
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; } = null!;
}