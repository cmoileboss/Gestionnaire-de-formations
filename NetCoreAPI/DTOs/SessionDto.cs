using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant une session de formation (date, lieu et formation associée).
/// </summary>
public class SessionDto
{
    /// <summary>Identifiant unique de la session.</summary>
    public int SessionId { get; set; }

    /// <summary>Identifiant de la formation dont fait partie la session.</summary>
    [Required(ErrorMessage = "La formation est obligatoire")]
    public int FormationId { get; set; }

    /// <summary>Date et heure de début de la session.</summary>
    [Required(ErrorMessage = "La date de début est obligatoire")]
    public DateTime StartDate { get; set; }

    /// <summary>Date et heure de fin de la session.</summary>
    [Required(ErrorMessage = "La date de fin est obligatoire")]
    public DateTime EndDate { get; set; }

    /// <summary>Lieu où se déroule la session.</summary>
    [Required(ErrorMessage = "Le lieu est obligatoire")]
    [StringLength(255, ErrorMessage = "Le lieu ne doit pas dépasser 255 caractères")]
    public string Place { get; set; } = null!;
}