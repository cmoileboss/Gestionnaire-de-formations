namespace NetCoreAPI.Services
{
    using System.Threading.Tasks;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Utils;

    /// <summary>
    /// Interface du service d'authentification.
    /// Définit les méthodes pour gérer l'authentification des utilisateurs.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authentifie un utilisateur avec ses identifiants et génère un token JWT.
        /// </summary>
        /// <param name="loginDto">Données de connexion de l'utilisateur.</param>
        /// <returns>Result contenant le token JWT en cas de succès, ou un message d'erreur en cas d'échec.</returns>
        Task<Result<string>> Login(AuthDto loginDto);

        /// <summary>
        /// Crée un utilisateur avec ses identifiants.
        /// </summary>
        /// <param name="registerDto">Données d'inscription de l'utilisateur.</param>
        /// <returns>Result contenant l'utilisateur créé en cas de succès, ou un message d'erreur en cas d'échec.</returns>
        Task<Result<UserDto>> Register(AuthDto registerDto);
    }
}