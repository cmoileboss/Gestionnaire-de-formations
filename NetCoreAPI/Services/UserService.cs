using NetCoreAPI.Repositories;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Utils;
using AutoMapper;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des utilisateurs.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IResultRepository _resultRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service utilisateur.
    /// </summary>
    /// <param name="userRepository">Repository utilisateur injecté.</param>
    /// <param name="subscriptionRepository">Repository abonnement injecté.</param>
    /// <param name="sessionRepository">Repository session injecté.</param>
    /// <param name="resultRepository">Repository résultat injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public UserService(
        IUserRepository userRepository,
        ISubscriptionRepository subscriptionRepository,
        ISessionRepository sessionRepository,
        IResultRepository resultRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        _sessionRepository = sessionRepository;
        _resultRepository = resultRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Récupère tous les utilisateurs.
    /// </summary>
    /// <returns>Result contenant la liste des DTO utilisateurs.</returns>
    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Result<IEnumerable<UserDto>>.Success(userDtos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving users.", ex);
        }
    }

    /// <summary>
    /// Récupère un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur.</param>
    /// <returns>Result contenant le DTO utilisateur ou un message d'erreur.</returns>
    public async Task<Result<UserDto>> GetByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result<UserDto>.Failure($"User with ID {id} not found.");

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving user.", ex);
        }
    }

    /// <summary>
    /// Récupère un utilisateur par son email.
    /// </summary>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <returns>Result contenant le DTO utilisateur ou un message d'erreur.</returns>
    public async Task<Result<UserDto>> GetByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return Result<UserDto>.Failure($"User with email {email} not found.");

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving user by email.", ex);
        }
    }

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="userDto">DTO utilisateur à créer.</param>
    /// <returns>Result contenant le DTO utilisateur créé ou un message d'erreur.</returns>
    public async Task<Result<UserDto>> CreateAsync(UserCreationUpdateDto userDto)
    {
        try
        {
            if (await _userRepository.EmailExistsAsync(userDto.Email))
                return Result<UserDto>.Failure($"Email {userDto.Email} is already in use.");

            var user = _mapper.Map<User>(userDto);
            var createdUser = await _userRepository.CreateAsync(user);
            var createdUserDto = _mapper.Map<UserDto>(createdUser);
            return Result<UserDto>.Success(createdUserDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating user.", ex);
        }
    }

    /// <summary>
    /// Met à jour un utilisateur existant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à mettre à jour.</param>
    /// <param name="userDto">DTO utilisateur à mettre à jour.</param>
    /// <returns>Result contenant le DTO utilisateur mis à jour ou un message d'erreur.</returns>
    public async Task<Result<UserDto>> UpdateAsync(int id, UserCreationUpdateDto userDto)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(id))
                return Result<UserDto>.Failure($"User with ID {id} not found.");

            var user = _mapper.Map<User>(userDto);
            user.UserId = id;
            var updatedUser = await _userRepository.UpdateAsync(user);
            var updatedUserDto = _mapper.Map<UserDto>(updatedUser);
            return Result<UserDto>.Success(updatedUserDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating user.", ex);
        }
    }

    /// <summary>
    /// Supprime un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à supprimer.</param>
    /// <returns>Result indiquant le succès ou l'échec de la suppression.</returns>
    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(id))
                return Result<bool>.Failure($"User with ID {id} not found.");

            var deleted = await _userRepository.DeleteAsync(id);
            return Result<bool>.Success(deleted);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while deleting user.", ex);
        }
    }

    /// <summary>
    /// Récupère toutes les sessions auxquelles un utilisateur est inscrit.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Result contenant la liste des sessions ou un message d'erreur.</returns>
    public async Task<Result<IEnumerable<SessionDto>>> GetUserSessionsAsync(int userId)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(userId))
                return Result<IEnumerable<SessionDto>>.Failure($"User with ID {userId} not found.");

            var subscriptions = await _subscriptionRepository.GetByUserIdAsync(userId);
            var sessions = subscriptions.Select(s => s.Session).ToList();
            var sessionDtos = _mapper.Map<IEnumerable<SessionDto>>(sessions);
            return Result<IEnumerable<SessionDto>>.Success(sessionDtos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving user sessions.", ex);
        }
    }

    /// <summary>
    /// Inscrit un utilisateur à une session de formation.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result indiquant le succès ou l'échec de l'inscription.</returns>
    public async Task<Result<bool>> SubscribeToSessionAsync(int userId, int sessionId)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(userId))
                return Result<bool>.Failure($"User with ID {userId} not found.");

            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null)
                return Result<bool>.Failure($"Session with ID {sessionId} not found.");

            var existing = await _subscriptionRepository.GetByUserAndSessionAsync(userId, sessionId);
            if (existing != null)
                return Result<bool>.Failure($"User {userId} is already subscribed to session {sessionId}.");

            var subscription = new Subscription
            {
                UserId = userId,
                SessionId = sessionId,
                SubscriptionDate = DateTime.UtcNow
            };
            await _subscriptionRepository.AddAsync(subscription);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while subscribing to session.", ex);
        }
    }

    /// <summary>
    /// Désinscrit un utilisateur d'une session de formation.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="sessionId">Identifiant de la session.</param>
    /// <returns>Result indiquant le succès ou l'échec de la désinscription.</returns>
    public async Task<Result<bool>> UnsubscribeFromSessionAsync(int userId, int sessionId)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetByUserAndSessionAsync(userId, sessionId);
            if (subscription == null)
                return Result<bool>.Failure($"Subscription not found for user {userId} and session {sessionId}.");

            await _subscriptionRepository.DeleteByCompositeKeyAsync(userId, sessionId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while unsubscribing from session.", ex);
        }
    }

    /// <summary>
    /// Récupère toutes les évaluations auxquelles un utilisateur est inscrit.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Result contenant la liste des évaluations ou un message d'erreur.</returns>
    public async Task<Result<IEnumerable<EvaluationDto>>> GetUserEvaluationsAsync(int userId)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(userId))
                return Result<IEnumerable<EvaluationDto>>.Failure($"User with ID {userId} not found.");

            var results = await _resultRepository.GetByUserIdAsync(userId);
            var evaluations = results.Select(r => r.Evaluation).Distinct().ToList();
            var evaluationDtos = _mapper.Map<IEnumerable<EvaluationDto>>(evaluations);
            return Result<IEnumerable<EvaluationDto>>.Success(evaluationDtos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving user evaluations.", ex);
        }
    }
}