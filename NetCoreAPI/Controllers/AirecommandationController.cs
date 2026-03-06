namespace NetCoreAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/airecommandations</c>.
    /// Gère les opérations CRUD sur les recommandations IA.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("airecommandations")]
    [Produces("application/json")]
    public class AirecommandationController : ControllerBase
    {
        private readonly IAirecommandationService _airecommandationService;

        /// <summary>
        /// Initialise le contrôleur avec le service recommandation IA injecté.
        /// </summary>
        /// <param name="airecommandationService">Service métier des recommandations IA.</param>
        public AirecommandationController(IAirecommandationService airecommandationService)
        {
            _airecommandationService = airecommandationService;
        }

        /// <summary>
        /// Récupère la liste de toutes les recommandations IA.
        /// </summary>
        /// <returns>200 OK avec la liste des recommandations IA.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AirecommandationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<AirecommandationDto>>> Get()
        {
            return Ok(await _airecommandationService.GetAllAsync());
        }

        /// <summary>
        /// Récupère une recommandation IA par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recommandation.</param>
        /// <returns>200 OK avec la recommandation ou 404 si non trouvée.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AirecommandationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AirecommandationDto>> GetById(int id)
        {
            try
            {
                var recommendation = await _airecommandationService.GetByIdAsync(id);
                return Ok(recommendation);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée une nouvelle recommandation IA.
        /// </summary>
        /// <param name="dto">Données de la recommandation à créer.</param>
        /// <returns>201 Created avec la recommandation créée.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AirecommandationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AirecommandationDto>> Create([FromBody] AirecommandationDto dto)
        {
            var created = await _airecommandationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.RecommendationId }, created);
        }

        /// <summary>
        /// Met à jour une recommandation IA existante.
        /// </summary>
        /// <param name="id">Identifiant de la recommandation à mettre à jour.</param>
        /// <param name="dto">Nouvelles données de la recommandation.</param>
        /// <returns>200 OK avec la recommandation mise à jour.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AirecommandationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AirecommandationDto>> Update(int id, [FromBody] AirecommandationDto dto)
        {
            try
            {
                var updated = await _airecommandationService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime une recommandation IA par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recommandation à supprimer.</param>
        /// <returns>204 No Content si supprimée, 404 si non trouvée.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _airecommandationService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
