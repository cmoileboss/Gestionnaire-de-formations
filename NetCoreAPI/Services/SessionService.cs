using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des sessions.
/// </summary>
public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service session.
    /// </summary>
    /// <param name="sessionRepository">Repository session injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public SessionService(ISessionRepository sessionRepository, IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SessionDto>>> GetAllAsync()
    {
        try
        {
            var sessions = await _sessionRepository.GetAllAsync();
            return Result<IEnumerable<SessionDto>>.Success(_mapper.Map<IEnumerable<SessionDto>>(sessions));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving sessions.", ex);
        }
    }

    public async Task<Result<SessionDto>> GetByIdAsync(int id)
    {
        try
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null)
                return Result<SessionDto>.Failure($"Session avec l'ID {id} non trouvée.");
            return Result<SessionDto>.Success(_mapper.Map<SessionDto>(session));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving session {id}.", ex);
        }
    }

    public async Task<Result<SessionDto>> CreateAsync(SessionDto dto)
    {
        try
        {
            var session = _mapper.Map<Session>(dto);
            await _sessionRepository.AddAsync(session);
            return Result<SessionDto>.Success(_mapper.Map<SessionDto>(session));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating session.", ex);
        }
    }

    public async Task<Result<SessionDto>> UpdateAsync(int id, SessionDto dto)
    {
        try
        {
            var existing = await _sessionRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<SessionDto>.Failure($"Session avec l'ID {id} non trouvée.");

            _mapper.Map(dto, existing);
            existing.SessionId = id;
            await _sessionRepository.UpdateAsync(existing);
            return Result<SessionDto>.Success(_mapper.Map<SessionDto>(existing));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating session {id}.", ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var existing = await _sessionRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<bool>.Failure($"Session avec l'ID {id} non trouvée.");

            await _sessionRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting session {id}.", ex);
        }
    }
}
