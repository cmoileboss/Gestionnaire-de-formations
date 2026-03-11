using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;
using NetCoreAPI.Utils;

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

    public async Task<Result<IEnumerable<EvaluationDto>>> GetAllAsync()
    {
        try
        {
            var evaluations = await _evaluationRepository.GetAllAsync();
            return Result<IEnumerable<EvaluationDto>>.Success(_mapper.Map<IEnumerable<EvaluationDto>>(evaluations));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving evaluations.", ex);
        }
    }

    public async Task<Result<EvaluationDto>> GetByIdAsync(int id)
    {
        try
        {
            var evaluation = await _evaluationRepository.GetByIdAsync(id);
            if (evaluation == null)
                return Result<EvaluationDto>.Failure($"Évaluation avec l'ID {id} non trouvée.");
            return Result<EvaluationDto>.Success(_mapper.Map<EvaluationDto>(evaluation));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving evaluation {id}.", ex);
        }
    }

    public async Task<Result<EvaluationDto>> CreateAsync(EvaluationDto dto)
    {
        try
        {
            var evaluation = _mapper.Map<Evaluation>(dto);
            await _evaluationRepository.AddAsync(evaluation);
            return Result<EvaluationDto>.Success(_mapper.Map<EvaluationDto>(evaluation));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating evaluation.", ex);
        }
    }

    public async Task<Result<EvaluationDto>> UpdateAsync(int id, EvaluationDto dto)
    {
        try
        {
            var existing = await _evaluationRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<EvaluationDto>.Failure($"Évaluation avec l'ID {id} non trouvée.");

            _mapper.Map(dto, existing);
            existing.EvaluationId = id;
            await _evaluationRepository.UpdateAsync(existing);
            return Result<EvaluationDto>.Success(_mapper.Map<EvaluationDto>(existing));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating evaluation {id}.", ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var existing = await _evaluationRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<bool>.Failure($"Évaluation avec l'ID {id} non trouvée.");

            await _evaluationRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting evaluation {id}.", ex);
        }
    }
}
