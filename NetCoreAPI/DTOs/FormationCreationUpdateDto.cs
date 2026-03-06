using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.DTOs;

public class FormationCreationUpdateDto
{
    [Required(ErrorMessage = "Le titre est obligatoire")]
    [StringLength(255, ErrorMessage = "Le titre ne doit pas dépasser 255 caractères")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "La description est obligatoire")]
    [StringLength(1000, ErrorMessage = "La description ne doit pas dépasser 1000 caractères")]
    public string Description { get; set; } = null!;
}