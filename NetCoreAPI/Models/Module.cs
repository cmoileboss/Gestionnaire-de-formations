
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente un module pédagogique faisant partie d'une ou plusieurs formations.
/// </summary>
[Table("Module")]
public partial class Module
{
    /// <summary>Identifiant unique du module.</summary>
    [Key]
    [Column("module_id")]
    public int ModuleId { get; set; }

    /// <summary>Titre du module.</summary>
    [Required(ErrorMessage = "Le titre est obligatoire")]
    [StringLength(255, ErrorMessage = "Le titre ne doit pas dépasser 255 caractères")]
    [Column("title")]
    public string Title { get; set; } = null!;

    /// <summary>Sujet principal abordé par le module.</summary>
    [Required(ErrorMessage = "Le sujet est obligatoire")]
    [StringLength(255, ErrorMessage = "Le sujet ne doit pas dépasser 255 caractères")]
    [Column("subject")]
    public string Subject { get; set; } = null!;

    /// <summary>Description détaillée du contenu du module.</summary>
    [Required(ErrorMessage = "La description est obligatoire")]
    [Column("description")]
    public string Description { get; set; } = null!;

    /// <summary>Évaluations rattachées à ce module.</summary>
    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    /// <summary>Formations qui incluent ce module.</summary>
    public virtual ICollection<Formation> Formations { get; set; } = new List<Formation>();
}
