namespace NetCoreAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/results</c>.
    /// Gère les opérations CRUD sur les résultats d'évaluation.
    /// La clé est composite : userId + evaluationId.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("results")]
    [Produces("application/json")]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        /// <summary>
        /// Initialise le contrôleur avec le service résultat injecté.
        /// </summary>
        /// <param name="resultService">Service métier des résultats.</param>
        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        /// <summary>
        /// Récupère la liste de tous les résultats.
        /// </summary>
        /// <returns>200 OK avec la liste des résultats.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResultDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ResultDto>>> Get()
        {
            return Ok(await _resultService.GetAllAsync());
        }

        /// <summary>
        /// Récupère un résultat par sa clé composite (userId + evaluationId).
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="evaluationId">Identifiant de l'évaluation.</param>
        /// <returns>200 OK avec le résultat ou 404 si non trouvé.</returns>
        [HttpGet("{userId}/{evaluationId}")]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResultDto>> GetById(int userId, int evaluationId)
        {
            try
            {
                var result = await _resultService.GetByIdAsync(userId, evaluationId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée un nouveau résultat.
        /// </summary>
        /// <param name="dto">Données du résultat à créer.</param>
        /// <returns>201 Created avec le résultat créé.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResultDto>> Create([FromBody] ResultDto dto)
        {
            try
            {
                var created = await _resultService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { userId = created.UserId, evaluationId = created.EvaluationId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Met à jour un résultat existant.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="evaluationId">Identifiant de l'évaluation.</param>
        /// <param name="dto">Nouvelles données du résultat.</param>
        /// <returns>200 OK avec le résultat mis à jour.</returns>
        [HttpPut("{userId}/{evaluationId}")]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResultDto>> Update(int userId, int evaluationId, [FromBody] ResultDto dto)
        {
            try
            {
                var updated = await _resultService.UpdateAsync(userId, evaluationId, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime un résultat par sa clé composite.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="evaluationId">Identifiant de l'évaluation.</param>
        /// <returns>204 No Content si supprimé, 404 si non trouvé.</returns>
        [HttpDelete("{userId}/{evaluationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int userId, int evaluationId)
        {
            try
            {
                await _resultService.DeleteAsync(userId, evaluationId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
