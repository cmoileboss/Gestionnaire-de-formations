namespace NetCoreAPI.Services
{
    using System.Threading.Tasks;
    using NetCoreAPI.DTOs;

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
        /// <returns>Token JWT si l'authentification réussit, sinon null.</returns>
        Task<string> Login(AuthDto loginDto);

        /// <summary>
        /// Crée un utilisateur avec ses identifiants.
        /// </summary>
        /// <param name="registerDto">Données d'inscription de l'utilisateur.</param>
        /// <returns>Utilisateur créé si l'inscription réussit, sinon null.</returns>
        Task<UserDto> Register(AuthDto registerDto);
    }
}