using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des résultats d'évaluation.
/// </summary>
public class ResultService : IResultService
{
    private readonly IResultRepository _resultRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service résultat.
    /// </summary>
    /// <param name="resultRepository">Repository résultat injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public ResultService(IResultRepository resultRepository, IMapper mapper)
    {
        _resultRepository = resultRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ResultDto>>> GetAllAsync()
    {
        try
        {
            var results = await _resultRepository.GetAllAsync();
            return Result<IEnumerable<ResultDto>>.Success(_mapper.Map<IEnumerable<ResultDto>>(results));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving results.", ex);
        }
    }

    public async Task<Result<ResultDto>> GetByIdAsync(int userId, int evaluationId)
    {
        try
        {
            var result = await _resultRepository.GetByIdAsync(userId, evaluationId);
            if (result == null)
                return Result<ResultDto>.Failure($"Résultat pour l'utilisateur {userId} et l'évaluation {evaluationId} non trouvé.");
            return Result<ResultDto>.Success(_mapper.Map<ResultDto>(result));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving result for user {userId} and evaluation {evaluationId}.", ex);
        }
    }

    public async Task<Result<ResultDto>> CreateAsync(ResultDto dto)
    {
        try
        {
            var existing = await _resultRepository.GetByIdAsync(dto.UserId, dto.EvaluationId);
            if (existing != null)
                return Result<ResultDto>.Failure($"Un résultat existe déjà pour l'utilisateur {dto.UserId} et l'évaluation {dto.EvaluationId}.");

            var result = _mapper.Map<Result>(dto);
            await _resultRepository.AddAsync(result);
            return Result<ResultDto>.Success(_mapper.Map<ResultDto>(result));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating result.", ex);
        }
    }

    public async Task<Result<ResultDto>> UpdateAsync(int userId, int evaluationId, ResultDto dto)
    {
        try
        {
            var existing = await _resultRepository.GetByIdAsync(userId, evaluationId);
            if (existing == null)
                return Result<ResultDto>.Failure($"Résultat pour l'utilisateur {userId} et l'évaluation {evaluationId} non trouvé.");

            existing.Score = dto.Score;
            existing.Success = dto.Success;
            existing.Date = dto.Date;
            await _resultRepository.UpdateAsync(existing);
            return Result<ResultDto>.Success(_mapper.Map<ResultDto>(existing));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating result for user {userId} and evaluation {evaluationId}.", ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int userId, int evaluationId)
    {
        try
        {
            var existing = await _resultRepository.GetByIdAsync(userId, evaluationId);
            if (existing == null)
                return Result<bool>.Failure($"Résultat pour l'utilisateur {userId} et l'évaluation {evaluationId} non trouvé.");

            await _resultRepository.DeleteAsync(userId, evaluationId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting result for user {userId} and evaluation {evaluationId}.", ex);
        }
    }
}
