
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente le résultat obtenu par un utilisateur à une évaluation.
/// Clé composite : identifiant utilisateur + identifiant évaluation.
/// </summary>
[Table("Result")]
public partial class Result
{
    /// <summary>Identifiant de l'utilisateur (partie de la clé composite).</summary>
    [Key]
    [Column("user_id", Order = 0)]
    [Required(ErrorMessage = "L'utilisateur est obligatoire")]
    public int UserId { get; set; }

    /// <summary>Identifiant de l'évaluation (partie de la clé composite).</summary>
    [Key]
    [Column("evaluation_id", Order = 1)]
    [Required(ErrorMessage = "L'évaluation est obligatoire")]
    public int EvaluationId { get; set; }

    /// <summary>Score numérique obtenu par l'utilisateur.</summary>
    [Required(ErrorMessage = "Le score est obligatoire")]
    [Column("score")]
    public double Score { get; set; }

    /// <summary>Indique si l'utilisateur a réussi l'évaluation.</summary>
    [Required(ErrorMessage = "Le succès est obligatoire")]
    [Column("success")]
    public bool Success { get; set; }

    /// <summary>Date à laquelle le résultat a été enregistré.</summary>
    [Required(ErrorMessage = "La date est obligatoire")]
    [Column("date")]
    public DateTime Date { get; set; }

    /// <summary>Évaluation associée à ce résultat.</summary>
    public virtual Evaluation Evaluation { get; set; } = null!;

    /// <summary>Utilisateur ayant obtenu ce résultat.</summary>
    public virtual User User { get; set; } = null!;
}
