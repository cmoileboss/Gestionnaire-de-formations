namespace NetCoreAPI.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetCoreAPI.DTOs;
    using NetCoreAPI.Services;

    /// <summary>
    /// Contrôleur REST exposé sous la route <c>/formations</c>.
    /// Gère les opérations CRUD sur les formations.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("formations")]
    [Produces("application/json")]
    public class FormationController : ControllerBase
    {
        private readonly IFormationService _formationService;

        /// <summary>
        /// Initialise le contrôleur avec le service formation injecté.
        /// </summary>
        /// <param name="formationService">Service métier des formations.</param>
        public FormationController(IFormationService formationService)
        {
            _formationService = formationService;
        }

        /// <summary>
        /// Récupère la liste de toutes les formations.
        /// </summary>
        /// <returns>200 OK avec la liste des formations.</returns>
        /// <response code="200">Liste des formations récupérée avec succès.</response>
        /// <response code="401">Non autorisé. L'utilisateur doit être authentifié.</response>
        /// <response code="403">Interdit. L'utilisateur n'a pas les permissions nécessaires.</response>
        /// <response code="500">Erreur serveur. Un problème est survenu lors de la récupération des formations.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<FormationDto>>> Get()
        {
            var formations = await _formationService.GetAllAsync();
            return Ok(formations);
        }

        /// <summary>
        /// Récupère une formation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <returns>200 OK avec la formation ou 404 si non trouvée.</returns>
        /// <response code="200">Formation récupérée avec succès.</response>
        /// <response code="401">Non autorisé.</response>
        /// <response code="404">Formation non trouvée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FormationDto>> GetById(int id)
        {
            try
            {
                var formation = await _formationService.GetByIdAsync(id);
                return Ok(formation);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Crée une nouvelle formation.
        /// </summary>
        /// <param name="formationDto">Données de la formation à créer.</param>
        /// <returns>201 Created avec la formation créée.</returns>
        /// <response code="201">Formation créée avec succès.</response>
        /// <response code="400">Données invalides.</response>
        /// <response code="401">Non autorisé.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FormationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<FormationDto>> Create([FromBody] FormationCreationUpdateDto formationDto)
        {
            var created = await _formationService.CreateAsync(formationDto);
            if (!created.IsSuccess)
                return BadRequest(new { error = created.Error });
            return CreatedAtAction(nameof(GetById), new { id = created.Value!.FormationId }, created.Value);
        }

        /// <summary>
        /// Met à jour une formation existante.
        /// </summary>
        /// <param name="id">Identifiant de la formation à mettre à jour.</param>
        /// <param name="formationDto">Nouvelles données de la formation.</param>
        /// <returns>200 OK avec la formation mise à jour.</returns>
        /// <response code="200">Formation mise à jour avec succès.</response>
        /// <response code="400">Données invalides.</response>
        /// <response code="401">Non autorisé.</response>
        /// <response code="404">Formation non trouvée.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FormationDto>> Update(int id, [FromBody] FormationCreationUpdateDto formationDto)
        {
            try
            {
                var updated = await _formationService.UpdateAsync(id, formationDto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Supprime une formation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation à supprimer.</param>
        /// <returns>204 No Content si supprimée, 404 si non trouvée.</returns>
        /// <response code="204">Formation supprimée avec succès.</response>
        /// <response code="401">Non autorisé.</response>
        /// <response code="404">Formation non trouvée.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _formationService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Récupère tous les modules associés à une formation.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <returns>200 OK avec la liste des modules.</returns>
        [HttpGet("{id}/modules")]
        [ProducesResponseType(typeof(IEnumerable<ModuleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetFormationModules(int id)
        {
            var result = await _formationService.GetFormationModulesAsync(id);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Associe un module à une formation.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <param name="moduleId">Identifiant du module.</param>
        /// <returns>200 OK si l'association réussit.</returns>
        [HttpPost("{id}/modules/{moduleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddModuleToFormation(int id, int moduleId)
        {
            var result = await _formationService.AddModuleToFormationAsync(id, moduleId);
            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(new { message = "Module ajouté à la formation avec succès" });
        }

        /// <summary>
        /// Retire un module d'une formation.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <param name="moduleId">Identifiant du module.</param>
        /// <returns>200 OK si le retrait réussit.</returns>
        [HttpDelete("{id}/modules/{moduleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveModuleFromFormation(int id, int moduleId)
        {
            var result = await _formationService.RemoveModuleFromFormationAsync(id, moduleId);
            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return Ok(new { message = "Module retiré de la formation avec succès" });
        }
    }
}