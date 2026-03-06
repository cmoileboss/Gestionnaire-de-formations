using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

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

    public async Task<IEnumerable<SessionDto>> GetAllAsync()
    {
        var sessions = await _sessionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SessionDto>>(sessions);
    }

    public async Task<SessionDto?> GetByIdAsync(int id)
    {
        var session = await _sessionRepository.GetByIdAsync(id);
        if (session == null)
            throw new KeyNotFoundException($"Session avec l'ID {id} non trouvée.");
        return _mapper.Map<SessionDto>(session);
    }

    public async Task<SessionDto> CreateAsync(SessionDto dto)
    {
        var session = _mapper.Map<Session>(dto);
        await _sessionRepository.AddAsync(session);
        return _mapper.Map<SessionDto>(session);
    }

    public async Task<SessionDto> UpdateAsync(int id, SessionDto dto)
    {
        var existing = await _sessionRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Session avec l'ID {id} non trouvée.");

        _mapper.Map(dto, existing);
        existing.SessionId = id;
        await _sessionRepository.UpdateAsync(existing);
        return _mapper.Map<SessionDto>(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _sessionRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Session avec l'ID {id} non trouvée.");

        await _sessionRepository.DeleteAsync(id);
        return true;
    }
}
