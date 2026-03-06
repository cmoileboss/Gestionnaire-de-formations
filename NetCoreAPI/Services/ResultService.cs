using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

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

    public async Task<IEnumerable<ResultDto>> GetAllAsync()
    {
        var results = await _resultRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ResultDto>>(results);
    }

    public async Task<ResultDto?> GetByIdAsync(int userId, int evaluationId)
    {
        var result = await _resultRepository.GetByIdAsync(userId, evaluationId);
        if (result == null)
            throw new KeyNotFoundException($"Résultat pour l'utilisateur {userId} et l'évaluation {evaluationId} non trouvé.");
        return _mapper.Map<ResultDto>(result);
    }

    public async Task<ResultDto> CreateAsync(ResultDto dto)
    {
        var existing = await _resultRepository.GetByIdAsync(dto.UserId, dto.EvaluationId);
        if (existing != null)
            throw new InvalidOperationException($"Un résultat existe déjà pour l'utilisateur {dto.UserId} et l'évaluation {dto.EvaluationId}.");

        var result = _mapper.Map<Result>(dto);
        await _resultRepository.AddAsync(result);
        return _mapper.Map<ResultDto>(result);
    }

    public async Task<ResultDto> UpdateAsync(int userId, int evaluationId, ResultDto dto)
    {
        var existing = await _resultRepository.GetByIdAsync(userId, evaluationId);
        if (existing == null)
            throw new KeyNotFoundException($"Résultat pour l'utilisateur {userId} et l'évaluation {evaluationId} non trouvé.");

        existing.Score = dto.Score;
        existing.Success = dto.Success;
        existing.Date = dto.Date;
        await _resultRepository.UpdateAsync(existing);
        return _mapper.Map<ResultDto>(existing);
    }

    public async Task<bool> DeleteAsync(int userId, int evaluationId)
    {
        var existing = await _resultRepository.GetByIdAsync(userId, evaluationId);
        if (existing == null)
            throw new KeyNotFoundException($"Résultat pour l'utilisateur {userId} et l'évaluation {evaluationId} non trouvé.");

        await _resultRepository.DeleteAsync(userId, evaluationId);
        return true;
    }
}
