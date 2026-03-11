using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des recommandations IA.
/// </summary>
public class AirecommandationService : IAirecommandationService
{
    private readonly IAirecommandationRepository _airecommandationRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service recommandation IA.
    /// </summary>
    /// <param name="airecommandationRepository">Repository recommandation injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public AirecommandationService(IAirecommandationRepository airecommandationRepository, IMapper mapper)
    {
        _airecommandationRepository = airecommandationRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<AirecommandationDto>>> GetAllAsync()
    {
        try
        {
            var recommendations = await _airecommandationRepository.GetAllAsync();
            return Result<IEnumerable<AirecommandationDto>>.Success(_mapper.Map<IEnumerable<AirecommandationDto>>(recommendations));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving AI recommendations.", ex);
        }
    }

    public async Task<Result<AirecommandationDto>> GetByIdAsync(int id)
    {
        try
        {
            var recommendation = await _airecommandationRepository.GetByIdAsync(id);
            if (recommendation == null)
                return Result<AirecommandationDto>.Failure($"Recommandation IA avec l'ID {id} non trouvée.");
            return Result<AirecommandationDto>.Success(_mapper.Map<AirecommandationDto>(recommendation));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving AI recommendation {id}.", ex);
        }
    }

    public async Task<Result<AirecommandationDto>> CreateAsync(AirecommandationDto dto)
    {
        try
        {
            var recommendation = _mapper.Map<Airecommandation>(dto);
            await _airecommandationRepository.AddAsync(recommendation);
            return Result<AirecommandationDto>.Success(_mapper.Map<AirecommandationDto>(recommendation));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating AI recommendation.", ex);
        }
    }

    public async Task<Result<AirecommandationDto>> UpdateAsync(int id, AirecommandationDto dto)
    {
        try
        {
            var existing = await _airecommandationRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<AirecommandationDto>.Failure($"Recommandation IA avec l'ID {id} non trouvée.");

            _mapper.Map(dto, existing);
            existing.RecommendationId = id;
            await _airecommandationRepository.UpdateAsync(existing);
            return Result<AirecommandationDto>.Success(_mapper.Map<AirecommandationDto>(existing));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating AI recommendation {id}.", ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var existing = await _airecommandationRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<bool>.Failure($"Recommandation IA avec l'ID {id} non trouvée.");

            await _airecommandationRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting AI recommendation {id}.", ex);
        }
    }
}
