namespace NetCoreAPI.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;
    using NetCoreAPI.Utils;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/users</c>.
    /// Gère les opérations CRUD sur les utilisateurs.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("users")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initialise le contrôleur avec le service utilisateur injecté.
        /// </summary>
        /// <param name="userService">Service métier des utilisateurs.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Récupère la liste de tous les utilisateurs.
        /// </summary>
        /// <returns>200 OK avec la liste des utilisateurs.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var result = await _userService.GetAllAsync();
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });
            
            return Ok(result.Value);
        }

        /// <summary>
        /// Récupère un utilisateur par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <returns>200 OK avec l'utilisateur, ou 404 s'il n'existe pas.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });
 
            return Ok(result.Value);
        }


        /// <summary>
        /// Met à jour un utilisateur existant.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à mettre à jour.</param>
        /// <param name="userDto">Nouvelles données de l'utilisateur.</param>
        /// <returns>200 OK avec le DTO mis à jour.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UserCreationUpdateDto userDto)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.UpdateAsync(id, userDto);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Supprime un utilisateur par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à supprimer.</param>
        /// <returns>204 No Content si supprimé, 404 s'il n'existe pas.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete(int id)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return NoContent();
        }

        /// <summary>
        /// Récupère toutes les sessions auxquelles un utilisateur est inscrit.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <returns>200 OK avec la liste des sessions.</returns>
        [HttpGet("{id}/sessions")]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetUserSessions(int id)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.GetUserSessionsAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Inscrit un utilisateur à une session de formation.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>200 OK si l'inscription réussit.</returns>
        [HttpPost("{id}/sessions/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> SubscribeToSession(int id, int sessionId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.SubscribeToSessionAsync(id, sessionId);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(new { message = "Utilisateur inscrit à la session avec succès" });
        }

        /// <summary>
        /// Désinscrit un utilisateur d'une session de formation.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>200 OK si la désinscription réussit.</returns>
        [HttpDelete("{id}/sessions/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UnsubscribeFromSession(int id, int sessionId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.UnsubscribeFromSessionAsync(id, sessionId);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(new { message = "Utilisateur désinscrit de la session avec succès" });
        }

        /// <summary>
        /// Récupère toutes les évaluations auxquelles un utilisateur est inscrit.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <returns>200 OK avec la liste des évaluations.</returns>
        [HttpGet("{id}/evaluations")]
        [ProducesResponseType(typeof(IEnumerable<EvaluationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetUserEvaluations(int id)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.GetUserEvaluationsAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Inscrit un utilisateur à une évaluation.
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur.</param>
        /// <param name="evaluationId">Identifiant de l'évaluation.</param>
        /// <returns>200 OK si l'inscription réussit.</returns>
        [HttpPost("{id}/evaluations/{evaluationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> EnrollInEvaluation(int id, int evaluationId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (currentUserId != id)
                return Forbid();

            var result = await _userService.EnrollInEvaluationAsync(id, evaluationId);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(new { message = "Utilisateur inscrit à l'évaluation avec succès" });
        }
    }
}