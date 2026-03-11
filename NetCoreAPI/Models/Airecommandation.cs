using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente une recommandation générée par l'IA pour une session de formation.
/// </summary>
[Table("AIRecommandation")]
public partial class Airecommandation
{
    /// <summary>Identifiant unique de la recommandation IA.</summary>
    [Key]
    [Column("recommendation_id")]
    public int RecommendationId { get; set; }

    /// <summary>Identifiant de la session pour laquelle la recommandation a été générée.</summary>
    [Required(ErrorMessage = "La session est obligatoire")]
    [Column("session_id")]
    public int SessionId { get; set; }

    /// <summary>Date de génération de la recommandation.</summary>
    [Required(ErrorMessage = "La date est obligatoire")]
    [Column("date")]
    public DateTime Date { get; set; }

    /// <summary>Niveau de confiance de la recommandation IA (entre 0 et 100).</summary>
    [Required(ErrorMessage = "Le niveau de confiance est obligatoire")]
    [Column("confidence")]
    public short Confidence { get; set; }

    /// <summary>Session pour laquelle cette recommandation a été produite.</summary>
    public virtual Session Session { get; set; } = null!;
}
