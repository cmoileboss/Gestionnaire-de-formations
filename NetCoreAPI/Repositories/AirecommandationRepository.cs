using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Repository pour la gestion des entités Airecommandation.
    /// Fournit des méthodes CRUD pour les recommandations IA.
    /// </summary>
    public class AirecommandationRepository : IAirecommandationRepository
    {
        private readonly GestionFormationContext _context;
        /// <summary>
        /// Initialise une nouvelle instance du repository Airecommandation.
        /// </summary>
        /// <param name="context">Contexte de base de données injecté.</param>
        public AirecommandationRepository(GestionFormationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Récupère toutes les recommandations IA.
        /// </summary>
        /// <returns>Liste des recommandations IA.</returns>
        public async Task<IEnumerable<Airecommandation>> GetAllAsync() => await _context.Airecommandations.ToListAsync();

        /// <summary>
        /// Récupère une recommandation IA par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recommandation.</param>
        /// <returns>Recommandation correspondante ou null.</returns>
        public async Task<Airecommandation?> GetByIdAsync(int id) => await _context.Airecommandations.FindAsync(id);

        /// <summary>
        /// Ajoute une nouvelle recommandation IA.
        /// </summary>
        /// <param name="entity">Recommandation à ajouter.</param>
        public async Task AddAsync(Airecommandation entity)
        {
            _context.Airecommandations.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour une recommandation IA existante.
        /// </summary>
        /// <param name="entity">Recommandation à mettre à jour.</param>
        public async Task UpdateAsync(Airecommandation entity)
        {
            _context.Airecommandations.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime une recommandation IA par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recommandation à supprimer.</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Airecommandations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
