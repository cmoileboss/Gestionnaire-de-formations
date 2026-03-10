using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Repositories;
using NetCoreAPI.Models;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Service d'authentification des utilisateurs.
/// Implémente les méthodes de connexion et d'inscription définies dans l'interface IAuthService.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    /// <summary>
    /// Initialise le service d'authentification avec les dépendances nécessaires.
    /// </summary>
    /// <param name="userRepository">Dépôt d'utilisateurs pour accéder aux données utilisateur.</param>
    /// <param name="mapper">Mapper pour convertir entre les entités et les DTOs.</param>
    /// <param name="config">Configuration pour accéder aux paramètres de l'application, notamment pour le JWT.</param>
    public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration config)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _config = config;
    }


    /// <summary>
    /// Authentifie un utilisateur avec ses identifiants et génère un token JWT.
    /// Vérifie que l'email existe et que le mot de passe est correct, puis génère un token JWT contenant les claims de l'utilisateur.
    /// </summary>
    /// <param name="loginDto">Données de connexion de l'utilisateur.</param>
    /// <returns>Token JWT si l'authentification réussit, sinon null.</returns>
    public Task<string> Login(AuthDto loginDto)
    {
        // On vérifie que l'email existe et que le mot de passe est correct'
        var user = _userRepository.GetByEmailAsync(loginDto.Email).Result;
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        // Génération du token JWT
        var tokenString = TokenManager.CreateToken(user, this._config);
        return Task.FromResult(tokenString);
    }

    /// <summary>
    /// Inscrit un nouvel utilisateur avec les données fournies.
    /// Vérifie que l'email n'est pas déjà utilisé, puis crée l'utilisateur et retourne ses données.
    /// </summary>
    /// <param name="registerDto">Données d'inscription de l'utilisateur.</param>
    /// <returns>Utilisateur créé si l'inscription réussit, sinon null.</returns>
    public Task<UserDto> Register(AuthDto registerDto)
    {
        if (_userRepository.GetByEmailAsync(registerDto.Email).Result != null)
            throw new InvalidOperationException("Email already in use.");

        string hash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = hash
        };
        var createdUser = _userRepository.CreateAsync(user).Result;
        return Task.FromResult(_mapper.Map<UserDto>(createdUser));
    }
}