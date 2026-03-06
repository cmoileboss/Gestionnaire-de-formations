using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant une recommandation générée par l'IA pour une session.
/// </summary>
public class AirecommandationDto
{
    /// <summary>Identifiant unique de la recommandation.</summary>
    public int RecommendationId { get; set; }

    /// <summary>Identifiant de la session concernée par la recommandation.</summary>
    [Required(ErrorMessage = "La session est obligatoire")]
    public int SessionId { get; set; }

    /// <summary>Date de génération de la recommandation.</summary>
    [Required(ErrorMessage = "La date est obligatoire")]
    public DateTime Date { get; set; }

    /// <summary>Niveau de confiance du modèle IA (entre 0 et 100).</summary>
    [Required(ErrorMessage = "Le niveau de confiance est obligatoire")]
    public short Confidence { get; set; }
}