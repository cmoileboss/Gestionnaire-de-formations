using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Repository pour la gestion des entités Module.
    /// Fournit des méthodes CRUD pour les modules de formation.
    /// </summary>
    public class ModuleRepository : IModuleRepository
    {
        private readonly GestionFormationContext _context;
        /// <summary>
        /// Initialise une nouvelle instance du repository Module.
        /// </summary>
        /// <param name="context">Contexte de base de données injecté.</param>
        public ModuleRepository(GestionFormationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Récupère tous les modules.
        /// </summary>
        /// <returns>Liste des modules.</returns>
        public async Task<IEnumerable<Module>> GetAllAsync() => await _context.Modules.ToListAsync();

        /// <summary>
        /// Récupère un module par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du module.</param>
        /// <returns>Module correspondant ou null.</returns>
        public async Task<Module?> GetByIdAsync(int id) => await _context.Modules.FindAsync(id);

        /// <summary>
        /// Ajoute un nouveau module.
        /// </summary>
        /// <param name="entity">Module à ajouter.</param>
        public async Task AddAsync(Module entity)
        {
            _context.Modules.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour un module existant.
        /// </summary>
        /// <param name="entity">Module à mettre à jour.</param>
        public async Task UpdateAsync(Module entity)
        {
            _context.Modules.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime un module par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du module à supprimer.</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Modules.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
