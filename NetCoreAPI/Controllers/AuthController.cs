
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.DTOs;
using NetCoreAPI.Services;
using NetCoreAPI.Utils;

[ApiController]
[Route("")]
[Produces("application/json")]
public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly LDAPService _ldapService;

        public AuthController(IAuthService authService, LDAPService ldapService)
        {
            _authService = authService;
            _ldapService = ldapService;
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
            var result = await _authService.Login(loginDto);
            
            if (!result.IsSuccess)
                return Unauthorized(new { error = result.Error });

            Response.Cookies.Append("access_token", result.Value!, new CookieOptions
            {
                HttpOnly = true,   // non accessible en JS
                Secure = true,     // HTTPS uniquement
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
            
            return Ok(result.Value);
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
            var result = await _authService.Register(registerDto);
            
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });
            
            return Ok(result.Value);
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

        [HttpPost("ldap-login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LdapLogin([FromBody] LDAPDto ldapDto)
        {
            var token = _ldapService.Authenticate(ldapDto.Username, ldapDto.Password);
            if (token == null)
                return Unauthorized("Invalid LDAP credentials");

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,   // non accessible en JS
                Secure = true,     // HTTPS uniquement
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(token);
        }
    }