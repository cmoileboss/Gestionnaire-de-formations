
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.DTOs;
using NetCoreAPI.Services;

[ApiController]
[Route("")]
[Produces("application/json")]
public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authentifie un utilisateur et génère un token JWT.
        /// </summary>
        /// <param name="loginDto">Données de connexion de l'utilisateur.</param>
        /// <returns>200 OK avec le token JWT, ou 401 Unauthorized si les informations sont invalides.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] AuthDto loginDto)
        {
            var token = await _authService.Login(loginDto);
            if (token == null)
                return Unauthorized();

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,   // non accessible en JS
                Secure = true,     // HTTPS uniquement
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok();
        }

        /// <summary>
        /// Inscrit un nouvel utilisateur et génère un token JWT.
        /// </summary>
        /// <param name="registerDto">Données d'inscription de l'utilisateur.</param>
        /// <returns>200 OK avec le token JWT, ou 400 Bad Request si les données sont invalides.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register([FromBody] AuthDto registerDto)
        {
            var user = await _authService.Register(registerDto);
            return Ok(user);
        }

        /// <summary>
        /// Déconnecte l'utilisateur en supprimant le cookie JWT.
        /// </summary>
        /// <returns>200 OK après suppression du cookie.</returns>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok();
        }
    }