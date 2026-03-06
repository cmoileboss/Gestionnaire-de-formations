using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des évaluations.
/// </summary>
public class EvaluationService : IEvaluationService
{
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service évaluation.
    /// </summary>
    /// <param name="evaluationRepository">Repository évaluation injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public EvaluationService(IEvaluationRepository evaluationRepository, IMapper mapper)
    {
        _evaluationRepository = evaluationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EvaluationDto>> GetAllAsync()
    {
        var evaluations = await _evaluationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<EvaluationDto>>(evaluations);
    }

    public async Task<EvaluationDto?> GetByIdAsync(int id)
    {
        var evaluation = await _evaluationRepository.GetByIdAsync(id);
        if (evaluation == null)
            throw new KeyNotFoundException($"Évaluation avec l'ID {id} non trouvée.");
        return _mapper.Map<EvaluationDto>(evaluation);
    }

    public async Task<EvaluationDto> CreateAsync(EvaluationDto dto)
    {
        var evaluation = _mapper.Map<Evaluation>(dto);
        await _evaluationRepository.AddAsync(evaluation);
        return _mapper.Map<EvaluationDto>(evaluation);
    }

    public async Task<EvaluationDto> UpdateAsync(int id, EvaluationDto dto)
    {
        var existing = await _evaluationRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Évaluation avec l'ID {id} non trouvée.");

        _mapper.Map(dto, existing);
        existing.EvaluationId = id;
        await _evaluationRepository.UpdateAsync(existing);
        return _mapper.Map<EvaluationDto>(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _evaluationRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Évaluation avec l'ID {id} non trouvée.");

        await _evaluationRepository.DeleteAsync(id);
        return true;
    }
}
