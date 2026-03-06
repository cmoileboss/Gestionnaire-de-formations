namespace NetCoreAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/modules</c>.
    /// Gère les opérations CRUD sur les modules pédagogiques.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("modules")]
    [Produces("application/json")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        /// <summary>
        /// Initialise le contrôleur avec le service module injecté.
        /// </summary>
        /// <param name="moduleService">Service métier des modules.</param>
        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        /// <summary>
        /// Récupère la liste de tous les modules.
        /// </summary>
        /// <returns>200 OK avec la liste des modules.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ModuleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> Get()
        {
            return Ok(await _moduleService.GetAllAsync());
        }

        /// <summary>
        /// Récupère un module par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du module.</param>
        /// <returns>200 OK avec le module ou 404 si non trouvé.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleDto>> GetById(int id)
        {
            try
            {
                var module = await _moduleService.GetByIdAsync(id);
                return Ok(module);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée un nouveau module.
        /// </summary>
        /// <param name="dto">Données du module à créer.</param>
        /// <returns>201 Created avec le module créé.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleDto>> Create([FromBody] ModuleDto dto)
        {
            var created = await _moduleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ModuleId }, created);
        }

        /// <summary>
        /// Met à jour un module existant.
        /// </summary>
        /// <param name="id">Identifiant du module à mettre à jour.</param>
        /// <param name="dto">Nouvelles données du module.</param>
        /// <returns>200 OK avec le module mis à jour.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ModuleDto>> Update(int id, [FromBody] ModuleDto dto)
        {
            try
            {
                var updated = await _moduleService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime un module par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du module à supprimer.</param>
        /// <returns>204 No Content si supprimé, 404 si non trouvé.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _moduleService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
