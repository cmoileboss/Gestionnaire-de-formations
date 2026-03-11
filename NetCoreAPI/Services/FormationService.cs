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
}