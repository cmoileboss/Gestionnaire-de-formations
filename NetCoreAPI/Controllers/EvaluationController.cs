namespace NetCoreAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/evaluations</c>.
    /// Gère les opérations CRUD sur les évaluations.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("evaluations")]
    [Produces("application/json")]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;

        /// <summary>
        /// Initialise le contrôleur avec le service évaluation injecté.
        /// </summary>
        /// <param name="evaluationService">Service métier des évaluations.</param>
        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        /// <summary>
        /// Récupère la liste de toutes les évaluations.
        /// </summary>
        /// <returns>200 OK avec la liste des évaluations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EvaluationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<EvaluationDto>>> Get()
        {
            return Ok(await _evaluationService.GetAllAsync());
        }

        /// <summary>
        /// Récupère une évaluation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'évaluation.</param>
        /// <returns>200 OK avec l'évaluation ou 404 si non trouvée.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EvaluationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EvaluationDto>> GetById(int id)
        {
            try
            {
                var evaluation = await _evaluationService.GetByIdAsync(id);
                return Ok(evaluation);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée une nouvelle évaluation.
        /// </summary>
        /// <param name="dto">Données de l'évaluation à créer.</param>
        /// <returns>201 Created avec l'évaluation créée.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(EvaluationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EvaluationDto>> Create([FromBody] EvaluationDto dto)
        {
            var created = await _evaluationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.EvaluationId }, created);
        }

        /// <summary>
        /// Met à jour une évaluation existante.
        /// </summary>
        /// <param name="id">Identifiant de l'évaluation à mettre à jour.</param>
        /// <param name="dto">Nouvelles données de l'évaluation.</param>
        /// <returns>200 OK avec l'évaluation mise à jour.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EvaluationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EvaluationDto>> Update(int id, [FromBody] EvaluationDto dto)
        {
            try
            {
                var updated = await _evaluationService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime une évaluation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'évaluation à supprimer.</param>
        /// <returns>204 No Content si supprimée, 404 si non trouvée.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _evaluationService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
