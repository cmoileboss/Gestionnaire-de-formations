using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Repositories;
using NetCoreAPI.Models;
using NetCoreAPI.Utils;
using BCrypt.Net;

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
    /// <returns>Result contenant le token JWT en cas de succès, ou un message d'erreur en cas d'échec.</returns>
    public async Task<Result<string>> Login(AuthDto loginDto)
    {
        try
        {
            // On vérifie que l'email existe
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
                return Result<string>.Failure("Invalid email or password.");

            // Vérifier le mot de passe avec BCrypt
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Result<string>.Failure("Invalid email or password.");

            // Génération du token JWT
            var tokenString = TokenManager.CreateToken(user, this._config);
            return Result<string>.Success(tokenString);
        }
        catch (SaltParseException)
        {
            // Le hash en base de données n'est pas un hash BCrypt valide
            return Result<string>.Failure("Invalid password hash in database. Please contact an administrator.");
        }
        catch (Exception ex)
        {
            // Erreur inattendue (base de données inaccessible, etc.)
            throw new InvalidOperationException("An error occurred during login.", ex);
        }
    }

    /// <summary>
    /// Inscrit un nouvel utilisateur avec les données fournies.
    /// Vérifie que l'email n'est pas déjà utilisé, puis crée l'utilisateur et retourne ses données.
    /// </summary>
    /// <param name="registerDto">Données d'inscription de l'utilisateur.</param>
    /// <returns>Result contenant l'utilisateur créé en cas de succès, ou un message d'erreur en cas d'échec.</returns>
    public async Task<Result<UserDto>> Register(AuthDto registerDto)
    {
        try
        {
            // Vérifier si l'email est déjà utilisé
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return Result<UserDto>.Failure("Email already in use.");

            // Hasher le mot de passe
            string hash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = hash
            };
            
            // Créer l'utilisateur
            var createdUser = await _userRepository.CreateAsync(user);
            var userDto = _mapper.Map<UserDto>(createdUser);
            
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            // Erreur inattendue (base de données inaccessible, etc.)
            throw new InvalidOperationException("An error occurred during registration.", ex);
        }
    }
}