using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

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

    public async Task<IEnumerable<AirecommandationDto>> GetAllAsync()
    {
        var recommendations = await _airecommandationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AirecommandationDto>>(recommendations);
    }

    public async Task<AirecommandationDto?> GetByIdAsync(int id)
    {
        var recommendation = await _airecommandationRepository.GetByIdAsync(id);
        if (recommendation == null)
            throw new KeyNotFoundException($"Recommandation IA avec l'ID {id} non trouvée.");
        return _mapper.Map<AirecommandationDto>(recommendation);
    }

    public async Task<AirecommandationDto> CreateAsync(AirecommandationDto dto)
    {
        var recommendation = _mapper.Map<Airecommandation>(dto);
        await _airecommandationRepository.AddAsync(recommendation);
        return _mapper.Map<AirecommandationDto>(recommendation);
    }

    public async Task<AirecommandationDto> UpdateAsync(int id, AirecommandationDto dto)
    {
        var existing = await _airecommandationRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Recommandation IA avec l'ID {id} non trouvée.");

        _mapper.Map(dto, existing);
        existing.RecommendationId = id;
        await _airecommandationRepository.UpdateAsync(existing);
        return _mapper.Map<AirecommandationDto>(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _airecommandationRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Recommandation IA avec l'ID {id} non trouvée.");

        await _airecommandationRepository.DeleteAsync(id);
        return true;
    }
}
