using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

namespace NetCoreAPI.Services;

/// <summary>
///  Service métier pour la gestion des formations.
/// </summary>
public class FormationService : IFormationService
{
    private readonly IFormationRepository _formationRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service de formation.
    /// </summary>
    /// <param name="formationRepository">Repository de formation injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public FormationService(IFormationRepository formationRepository, IMapper mapper)
    {
        _formationRepository = formationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FormationDto>> GetAllAsync()
    {
        var formations = await _formationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FormationDto>>(formations);
    }

    public async Task<FormationDto?> GetByIdAsync(int id)
    {
        var formation = await _formationRepository.GetByIdAsync(id);
        if (formation == null)
            throw new KeyNotFoundException($"Formation avec l'ID {id} non trouvée.");
        return _mapper.Map<FormationDto>(formation);
    }

    public async Task<FormationDto> CreateAsync(FormationCreationUpdateDto formationDto)
    {
        var formation = _mapper.Map<Formation>(formationDto);
        await _formationRepository.AddAsync(formation);
        return _mapper.Map<FormationDto>(formation);
    }

    public async Task<FormationDto> UpdateAsync(int id, FormationCreationUpdateDto formationDto)
    {
        var existingFormation = await _formationRepository.GetByIdAsync(id);
        if (existingFormation == null)
        {
            throw new KeyNotFoundException($"Formation avec l'ID {id} non trouvée.");
        }

        _mapper.Map(formationDto, existingFormation);
        await _formationRepository.UpdateAsync(existingFormation);
        return _mapper.Map<FormationDto>(existingFormation);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var exists = await _formationRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"Formation avec l'ID {id} non trouvée.");

        await _formationRepository.DeleteAsync(id);
        return true;
    }
}