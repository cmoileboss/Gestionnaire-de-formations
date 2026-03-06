using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

/// <summary>
/// DTO représentant un module pédagogique.
/// </summary>
public class ModuleDto
{
    /// <summary>Identifiant unique du module.</summary>
    public int ModuleId { get; set; }

    /// <summary>Titre du module.</summary>
    [Required(ErrorMessage = "Le titre est obligatoire")]
    [StringLength(255, ErrorMessage = "Le titre ne doit pas dépasser 255 caractères")]
    public string Title { get; set; } = null!;

    /// <summary>Sujet principal abordé par le module.</summary>
    [Required(ErrorMessage = "Le sujet est obligatoire")]
    [StringLength(255, ErrorMessage = "Le sujet ne doit pas dépasser 255 caractères")]
    public string Subject { get; set; } = null!;

    /// <summary>Description détaillée du contenu du module.</summary>
    [Required(ErrorMessage = "La description est obligatoire")]
    public string Description { get; set; } = null!;
}