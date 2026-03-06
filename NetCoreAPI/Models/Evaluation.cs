
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente une évaluation associée à un module de formation.
/// </summary>
[Table("Evaluation")]
public partial class Evaluation
{
    /// <summary>Identifiant unique de l'évaluation.</summary>
    [Key]
    [Column("evaluation_id")]
    public int EvaluationId { get; set; }

    /// <summary>Date et heure de début de l'évaluation.</summary>
    [Required(ErrorMessage = "La date de début est obligatoire")]
    [Column("start_date")]
    public DateTime StartDate { get; set; }

    /// <summary>Date et heure de fin de l'évaluation.</summary>
    [Required(ErrorMessage = "La date de fin est obligatoire")]
    [Column("end_date")]
    public DateTime EndDate { get; set; }

    /// <summary>Lieu où se déroule l'évaluation.</summary>
    [Required(ErrorMessage = "Le lieu est obligatoire")]
    [StringLength(255, ErrorMessage = "Le lieu ne doit pas dépasser 255 caractères")]
    [Column("place")]
    public string Place { get; set; } = null!;

    /// <summary>Identifiant du module auquel cette évaluation est rattachée.</summary>
    [Required(ErrorMessage = "Le module est obligatoire")]
    [Column("module_id")]
    public int ModuleId { get; set; }

    /// <summary>Module auquel cette évaluation est rattachée.</summary>
    public virtual Module Module { get; set; } = null!;

    /// <summary>Résultats obtenus par les utilisateurs à cette évaluation.</summary>
    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
