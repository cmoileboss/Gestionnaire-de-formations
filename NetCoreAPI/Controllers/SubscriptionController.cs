namespace NetCoreAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/subscriptions</c>.
    /// Gère les opérations CRUD sur les abonnements (inscriptions aux sessions).
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("subscriptions")]
    [Produces("application/json")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        /// <summary>
        /// Initialise le contrôleur avec le service abonnement injecté.
        /// </summary>
        /// <param name="subscriptionService">Service métier des abonnements.</param>
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Récupère la liste de tous les abonnements.
        /// </summary>
        /// <returns>200 OK avec la liste des abonnements.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SubscriptionDto>>> Get()
        {
            return Ok(await _subscriptionService.GetAllAsync());
        }

        /// <summary>
        /// Récupère un abonnement par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement.</param>
        /// <returns>200 OK avec l'abonnement ou 404 si non trouvé.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SubscriptionDto>> GetById(int id)
        {
            try
            {
                var subscription = await _subscriptionService.GetByIdAsync(id);
                return Ok(subscription);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée un nouvel abonnement.
        /// </summary>
        /// <param name="dto">Données de l'abonnement à créer.</param>
        /// <returns>201 Created avec l'abonnement créé.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SubscriptionDto>> Create([FromBody] SubscriptionDto dto)
        {
            var created = await _subscriptionService.CreateAsync(dto);
            if (!created.IsSuccess)
                return BadRequest(new { error = created.Error });
            return CreatedAtAction(nameof(GetById), new { id = created.Value!.UserId }, created.Value);
        }

        /// <summary>
        /// Met à jour un abonnement existant.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement à mettre à jour.</param>
        /// <param name="dto">Nouvelles données de l'abonnement.</param>
        /// <returns>200 OK avec l'abonnement mis à jour.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SubscriptionDto>> Update(int id, [FromBody] SubscriptionDto dto)
        {
            try
            {
                var updated = await _subscriptionService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime un abonnement par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement à supprimer.</param>
        /// <returns>204 No Content si supprimé, 404 si non trouvé.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _subscriptionService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Récupère tous les abonnements d'un utilisateur.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <returns>200 OK avec la liste des abonnements.</returns>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetByUserId(int userId)
        {
            var result = await _subscriptionService.GetByUserIdAsync(userId);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Récupère tous les abonnements d'une session.
        /// </summary>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>200 OK avec la liste des abonnements.</returns>
        [HttpGet("session/{sessionId}")]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetBySessionId(int sessionId)
        {
            var result = await _subscriptionService.GetBySessionIdAsync(sessionId);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Récupère un abonnement spécifique par utilisateur et session.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>200 OK avec l'abonnement ou 404 si non trouvé.</returns>
        [HttpGet("{userId}/{sessionId}")]
        [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SubscriptionDto>> GetByUserAndSession(int userId, int sessionId)
        {
            var result = await _subscriptionService.GetByUserAndSessionAsync(userId, sessionId);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Supprime un abonnement par utilisateur et session.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>200 OK si supprimé, 404 si non trouvé.</returns>
        [HttpDelete("{userId}/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteByUserAndSession(int userId, int sessionId)
        {
            var result = await _subscriptionService.DeleteByUserAndSessionAsync(userId, sessionId);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(new { message = "Abonnement supprimé avec succès" });
        }
    }
}
