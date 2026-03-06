using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant une formation avec ses modules et sessions associés.
/// </summary>
public class FormationDto
{
    /// <summary>Identifiant unique de la formation.</summary>
    public int FormationId { get; set; }

    /// <summary>Titre de la formation.</summary>
    [Required(ErrorMessage = "Le titre est obligatoire")]
    [StringLength(255, ErrorMessage = "Le titre ne doit pas dépasser 255 caractères")]
    public string Title { get; set; } = null!;

    /// <summary>Description détaillée de la formation.</summary>
    [Required(ErrorMessage = "La description est obligatoire")]
    public string Description { get; set; } = null!;

    /// <summary>Liste des modules composant la formation.</summary>
    public List<ModuleDto> Modules { get; set; } = new List<ModuleDto>();

    /// <summary>Liste des sessions planifiées pour cette formation.</summary>
    public List<SessionDto> Sessions { get; set; } = new List<SessionDto>();
}