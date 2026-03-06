
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models;

/// <summary>
/// Représente une session planifiée d'une formation à une date et un lieu donnés.
/// </summary>
[Table("Session")]
public partial class Session
{
    /// <summary>Identifiant unique de la session.</summary>
    [Key]
    [Column("session_id")]
    public int SessionId { get; set; }

    /// <summary>Identifiant de la formation associée à cette session.</summary>
    [Required(ErrorMessage = "La formation est obligatoire")]
    [Column("formation_id")]
    public int FormationId { get; set; }

    /// <summary>Date et heure de début de la session.</summary>
    [Required(ErrorMessage = "La date de début est obligatoire")]
    [Column("start_date")]
    public DateTime StartDate { get; set; }

    /// <summary>Date et heure de fin de la session.</summary>
    [Required(ErrorMessage = "La date de fin est obligatoire")]
    [Column("end_date")]
    public DateTime EndDate { get; set; }

    /// <summary>Lieu où se déroule la session.</summary>
    [Required(ErrorMessage = "Le lieu est obligatoire")]
    [StringLength(255, ErrorMessage = "Le lieu ne doit pas dépasser 255 caractères")]
    [Column("place")]
    public string Place { get; set; } = null!;

    /// <summary>Recommandations IA générées pour cette session.</summary>
    public virtual ICollection<Airecommandation> Airecommandations { get; set; } = new List<Airecommandation>();

    /// <summary>Formation associée à cette session.</summary>
    public virtual Formation Formation { get; set; } = null!;

    /// <summary>Abonnements des utilisateurs à cette session.</summary>
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
