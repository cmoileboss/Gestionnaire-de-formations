using NetCoreAPI.DTOs;

namespace NetCoreAPI.Services;

/// <summary>
/// Interface du service métier pour la gestion des formations.
/// </summary>
public interface IFormationService
{
    /// <summary>
    /// Récupère toutes les formations.
    /// </summary>
    /// <returns>Liste des DTO formations.</returns>
    Task<IEnumerable<FormationDto>> GetAllAsync();

    /// <summary>
    /// Récupère une formation par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de la formation.</param>
    /// <returns>Le DTO formation ou null.</returns>
    Task<FormationDto?> GetByIdAsync(int id);

    /// <summary>
    /// Crée une nouvelle formation.
    /// </summary>
    /// <param name="formationDto">DTO formation à créer.</param>
    /// <returns>Le DTO formation créé.</returns>
    Task<FormationDto> CreateAsync(FormationCreationUpdateDto formationDto);

    /// <summary>
    /// Met à jour une formation existante.
    /// </summary>
    /// <param name="id">Identifiant de la formation à mettre à jour.</param>
    /// <param name="formationDto">DTO formation à mettre à jour.</param>
    /// <returns>Le DTO formation mis à jour.</returns>
    Task<FormationDto> UpdateAsync(int id, FormationCreationUpdateDto formationDto);

    /// <summary>
    /// Supprime une formation par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de la formation à supprimer.</param>
    /// <returns>True si supprimé, sinon false.</returns>
    Task<bool> DeleteAsync(int id);
}