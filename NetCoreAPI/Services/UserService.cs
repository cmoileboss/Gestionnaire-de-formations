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
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service utilisateur.
    /// </summary>
    /// <param name="userRepository">Repository utilisateur injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
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
}