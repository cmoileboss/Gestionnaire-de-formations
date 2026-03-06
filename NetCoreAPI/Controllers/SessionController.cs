namespace NetCoreAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/sessions</c>.
    /// Gère les opérations CRUD sur les sessions de formation.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("sessions")]
    [Produces("application/json")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Initialise le contrôleur avec le service session injecté.
        /// </summary>
        /// <param name="sessionService">Service métier des sessions.</param>
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Récupère la liste de toutes les sessions.
        /// </summary>
        /// <returns>200 OK avec la liste des sessions.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SessionDto>>> Get()
        {
            return Ok(await _sessionService.GetAllAsync());
        }

        /// <summary>
        /// Récupère une session par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la session.</param>
        /// <returns>200 OK avec la session ou 404 si non trouvée.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SessionDto>> GetById(int id)
        {
            try
            {
                var session = await _sessionService.GetByIdAsync(id);
                return Ok(session);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée une nouvelle session.
        /// </summary>
        /// <param name="dto">Données de la session à créer.</param>
        /// <returns>201 Created avec la session créée.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SessionDto>> Create([FromBody] SessionDto dto)
        {
            var created = await _sessionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.SessionId }, created);
        }

        /// <summary>
        /// Met à jour une session existante.
        /// </summary>
        /// <param name="id">Identifiant de la session à mettre à jour.</param>
        /// <param name="dto">Nouvelles données de la session.</param>
        /// <returns>200 OK avec la session mise à jour.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SessionDto>> Update(int id, [FromBody] SessionDto dto)
        {
            try
            {
                var updated = await _sessionService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime une session par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la session à supprimer.</param>
        /// <returns>204 No Content si supprimée, 404 si non trouvée.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _sessionService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
