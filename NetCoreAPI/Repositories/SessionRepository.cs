using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Repository pour la gestion des entités Session.
    /// Fournit des méthodes CRUD pour les sessions de formation.
    /// </summary>
    public class SessionRepository : ISessionRepository
    {
        private readonly GestionFormationContext _context;
        /// <summary>
        /// Initialise une nouvelle instance du repository Session.
        /// </summary>
        /// <param name="context">Contexte de base de données injecté.</param>
        public SessionRepository(GestionFormationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Récupère toutes les sessions.
        /// </summary>
        /// <returns>Liste des sessions.</returns>
        public async Task<IEnumerable<Session>> GetAllAsync() => await _context.Sessions.ToListAsync();

        /// <summary>
        /// Récupère une session par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la session.</param>
        /// <returns>Session correspondante ou null.</returns>
        public async Task<Session?> GetByIdAsync(int id) => await _context.Sessions.FindAsync(id);

        /// <summary>
        /// Ajoute une nouvelle session.
        /// </summary>
        /// <param name="entity">Session à ajouter.</param>
        public async Task AddAsync(Session entity)
        {
            _context.Sessions.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour une session existante.
        /// </summary>
        /// <param name="entity">Session à mettre à jour.</param>
        public async Task UpdateAsync(Session entity)
        {
            _context.Sessions.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime une session par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la session à supprimer.</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Sessions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
