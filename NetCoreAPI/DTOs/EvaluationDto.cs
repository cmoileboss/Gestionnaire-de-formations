using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant une évaluation associée à un module.
/// </summary>
public class EvaluationDto
{
    /// <summary>Identifiant unique de l'évaluation.</summary>
    public int EvaluationId { get; set; }

    /// <summary>Date et heure de début de l'évaluation.</summary>
    [Required(ErrorMessage = "La date de début est obligatoire")]
    public DateTime StartDate { get; set; }

    /// <summary>Date et heure de fin de l'évaluation.</summary>
    [Required(ErrorMessage = "La date de fin est obligatoire")]
    public DateTime EndDate { get; set; }

    /// <summary>Lieu où se déroule l'évaluation.</summary>
    [Required(ErrorMessage = "Le lieu est obligatoire")]
    [StringLength(255, ErrorMessage = "Le lieu ne doit pas dépasser 255 caractères")]
    public string Place { get; set; } = null!;

    /// <summary>Identifiant du module auquel cette évaluation est rattachée.</summary>
    [Required(ErrorMessage = "Le module est obligatoire")]
    public int ModuleId { get; set; }
}