using NetCoreAPI.Repositories;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
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
    /// <returns>Liste des DTO utilisateurs.</returns>
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    /// <summary>
    /// Récupère un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur.</param>
    /// <returns>Le DTO utilisateur ou null.</returns>
    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found.");
        return _mapper.Map<UserDto?>(user);
    }

    /// <summary>
    /// Récupère un utilisateur par son email.
    /// </summary>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <returns>Le DTO utilisateur ou null.</returns>
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            throw new KeyNotFoundException($"User with email {email} not found.");
        return _mapper.Map<UserDto?>(user);
    }

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="userDto">DTO utilisateur à créer.</param>
    /// <returns>Le DTO utilisateur créé.</returns>
    public async Task<UserDto> CreateAsync(UserCreationUpdateDto userDto)
    {
        if (await _userRepository.EmailExistsAsync(userDto.Email))
            throw new InvalidOperationException($"Email {userDto.Email} is already in use.");
        var user = _mapper.Map<User>(userDto);
        var createdUser = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserDto>(createdUser);
    }

    /// <summary>
    /// Met à jour un utilisateur existant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à mettre à jour.</param>
    /// <param name="userDto">DTO utilisateur à mettre à jour.</param>
    /// <returns>Le DTO utilisateur mis à jour.</returns>
    public async Task<UserDto> UpdateAsync(int id, UserCreationUpdateDto userDto)
    {
        if (!await _userRepository.ExistsAsync(id))
            throw new KeyNotFoundException($"User with ID {id} not found.");
        var user = _mapper.Map<User>(userDto);
        user.UserId = id;
        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(updatedUser);
    }

    /// <summary>
    /// Supprime un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur à supprimer.</param>
    /// <returns>True si supprimé, sinon false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _userRepository.ExistsAsync(id))
            throw new KeyNotFoundException($"User with ID {id} not found.");
        return await _userRepository.DeleteAsync(id);
    }
}