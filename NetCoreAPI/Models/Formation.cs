
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente une formation disponible sur la plateforme.
/// </summary>
[Table("Formation")]
public partial class Formation
{
    /// <summary>Identifiant unique de la formation.</summary>
    [Key]
    [Column("formation_id")]
    public int FormationId { get; set; }

    /// <summary>Titre de la formation.</summary>
    [Required(ErrorMessage = "Le titre est obligatoire")]
    [StringLength(255, ErrorMessage = "Le titre ne doit pas dépasser 255 caractères")]
    [Column("title")]
    public string Title { get; set; } = null!;

    /// <summary>Description détaillée du contenu de la formation.</summary>
    [Required(ErrorMessage = "La description est obligatoire")]
    [Column("description")]
    public string Description { get; set; } = null!;

    /// <summary>Sessions planifiées pour cette formation.</summary>
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    /// <summary>Modules pédagogiques composant la formation.</summary>
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
}
