using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant le résultat d'un utilisateur à une évaluation.
/// Clé composite : UserId + EvaluationId.
/// </summary>
public class ResultDto
{
    /// <summary>Identifiant de l'utilisateur.</summary>
    [Required(ErrorMessage = "L'utilisateur est obligatoire")]
    public int UserId { get; set; }

    /// <summary>Identifiant de l'évaluation.</summary>
    [Required(ErrorMessage = "L'évaluation est obligatoire")]
    public int EvaluationId { get; set; }

    /// <summary>Score numérique obtenu par l'utilisateur.</summary>
    [Required(ErrorMessage = "Le score est obligatoire")]
    public double Score { get; set; }

    /// <summary>Indique si l'utilisateur a réussi l'évaluation.</summary>
    [Required(ErrorMessage = "Le succès est obligatoire")]
    public bool Success { get; set; }

    /// <summary>Date à laquelle le résultat a été enregistré.</summary>
    [Required(ErrorMessage = "La date est obligatoire")]
    public DateTime Date { get; set; }
}