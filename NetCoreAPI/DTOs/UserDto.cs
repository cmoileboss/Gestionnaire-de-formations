namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant les données publiques d'un utilisateur (lecture).
/// </summary>
public class UserDto
{
    /// <summary>Identifiant unique de l'utilisateur.</summary>
    public int UserId { get; set; }

    /// <summary>Adresse email de l'utilisateur.</summary>
    public string Email { get; set; } = null!;

    /// <summary>Adresse postale de l'utilisateur (optionnelle).</summary>
    public string? Address { get; set; }
}