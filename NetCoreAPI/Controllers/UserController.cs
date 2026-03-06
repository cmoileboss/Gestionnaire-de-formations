namespace NetCoreAPI.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

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
            return Ok(await _userService.GetAllAsync());
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

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
 
            return Ok(user);
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
            var updatedUser = await _userService.UpdateAsync(id, userDto);
            return Ok(updatedUser);
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

            var deleted = await _userService.DeleteAsync(id);
            if (deleted)
                return NoContent();
            else
                return NotFound();
        }
    }
}