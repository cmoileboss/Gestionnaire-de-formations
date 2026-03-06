using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Repository pour la gestion des entités Formation.
    /// Fournit des méthodes CRUD pour les formations.
    /// </summary>
    public class FormationRepository : IFormationRepository
    {
        private readonly GestionFormationContext _context;
        /// <summary>
        /// Initialise une nouvelle instance du repository Formation.
        /// </summary>
        /// <param name="context">Contexte de base de données injecté.</param>
        public FormationRepository(GestionFormationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Récupère toutes les formations.
        /// </summary>
        /// <returns>Liste des formations.</returns>
        public async Task<IEnumerable<Formation>> GetAllAsync() => await _context.Formations.ToListAsync();

        /// <summary>
        /// Récupère une formation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <returns>Formation correspondante ou null.</returns>
        public async Task<Formation?> GetByIdAsync(int id) => await _context.Formations.FindAsync(id);

        /// <summary>
        /// Ajoute une nouvelle formation.
        /// </summary>
        /// <param name="entity">Formation à ajouter.</param>
        public async Task AddAsync(Formation entity)
        {
            _context.Formations.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour une formation existante.
        /// </summary>
        /// <param name="entity">Formation à mettre à jour.</param>
        public async Task UpdateAsync(Formation entity)
        {
            _context.Formations.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime une formation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation à supprimer.</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Formations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary> 
        /// Vérifie si une formation existe par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la formation.</param>
        /// <returns>True si la formation existe, sinon false.</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Formations.AnyAsync(f => f.FormationId == id);
        }
    }
}
