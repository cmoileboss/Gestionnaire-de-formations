using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
///  Service métier pour la gestion des formations.
/// </summary>
public class FormationService : IFormationService
{
    private readonly IFormationRepository _formationRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service de formation.
    /// </summary>
    /// <param name="formationRepository">Repository de formation injecté.</param>
    /// <param name="moduleRepository">Repository de module injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public FormationService(
        IFormationRepository formationRepository,
        IModuleRepository moduleRepository,
        IMapper mapper)
    {
        _formationRepository = formationRepository;
        _moduleRepository = moduleRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<FormationDto>>> GetAllAsync()
    {
        try
        {
            var formations = await _formationRepository.GetAllAsync();
            return Result<IEnumerable<FormationDto>>.Success(_mapper.Map<IEnumerable<FormationDto>>(formations));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving formations.", ex);
        }
    }

    public async Task<Result<FormationDto>> GetByIdAsync(int id)
    {
        try
        {
            var formation = await _formationRepository.GetByIdAsync(id);
            if (formation == null)
                return Result<FormationDto>.Failure($"Formation avec l'ID {id} non trouvée.");
            return Result<FormationDto>.Success(_mapper.Map<FormationDto>(formation));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving formation {id}.", ex);
        }
    }

    public async Task<Result<FormationDto>> CreateAsync(FormationCreationUpdateDto formationDto)
    {
        try
        {
            var formation = _mapper.Map<Formation>(formationDto);
            await _formationRepository.AddAsync(formation);
            return Result<FormationDto>.Success(_mapper.Map<FormationDto>(formation));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating formation.", ex);
        }
    }

    public async Task<Result<FormationDto>> UpdateAsync(int id, FormationCreationUpdateDto formationDto)
    {
        try
        {
            var existingFormation = await _formationRepository.GetByIdAsync(id);
            if (existingFormation == null)
            {
                return Result<FormationDto>.Failure($"Formation avec l'ID {id} non trouvée.");
            }

            _mapper.Map(formationDto, existingFormation);
            await _formationRepository.UpdateAsync(existingFormation);
            return Result<FormationDto>.Success(_mapper.Map<FormationDto>(existingFormation));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating formation {id}.", ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var exists = await _formationRepository.ExistsAsync(id);
            if (!exists)
                return Result<bool>.Failure($"Formation avec l'ID {id} non trouvée.");

            await _formationRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting formation {id}.", ex);
        }
    }

    public async Task<Result<IEnumerable<ModuleDto>>> GetFormationModulesAsync(int formationId)
    {
        try
        {
            var formation = await _formationRepository.GetByIdWithModulesAsync(formationId);
            if (formation == null)
                return Result<IEnumerable<ModuleDto>>.Failure($"Formation avec l'ID {formationId} non trouvée.");

            var moduleDtos = _mapper.Map<IEnumerable<ModuleDto>>(formation.Modules);
            return Result<IEnumerable<ModuleDto>>.Success(moduleDtos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving modules for formation {formationId}.", ex);
        }
    }

    public async Task<Result<bool>> AddModuleToFormationAsync(int formationId, int moduleId)
    {
        try
        {
            var formation = await _formationRepository.GetByIdWithModulesAsync(formationId);
            if (formation == null)
                return Result<bool>.Failure($"Formation avec l'ID {formationId} non trouvée.");

            var module = await _moduleRepository.GetByIdAsync(moduleId);
            if (module == null)
                return Result<bool>.Failure($"Module avec l'ID {moduleId} non trouvé.");

            if (formation.Modules.Any(m => m.ModuleId == moduleId))
                return Result<bool>.Failure($"Le module {moduleId} est déjà associé à la formation {formationId}.");

            formation.Modules.Add(module);
            await _formationRepository.UpdateAsync(formation);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while adding module {moduleId} to formation {formationId}.", ex);
        }
    }

    public async Task<Result<bool>> RemoveModuleFromFormationAsync(int formationId, int moduleId)
    {
        try
        {
            var formation = await _formationRepository.GetByIdWithModulesAsync(formationId);
            if (formation == null)
                return Result<bool>.Failure($"Formation avec l'ID {formationId} non trouvée.");

            var module = formation.Modules.FirstOrDefault(m => m.ModuleId == moduleId);
            if (module == null)
                return Result<bool>.Failure($"Le module {moduleId} n'est pas associé à la formation {formationId}.");

            formation.Modules.Remove(module);
            await _formationRepository.UpdateAsync(formation);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while removing module {moduleId} from formation {formationId}.", ex);
        }
    }
}