using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Repository pour la gestion des abonnements (Subscription).
    /// </summary>
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly GestionFormationContext _context;

        /// <summary>
        /// Initialise une nouvelle instance du repository Subscription.
        /// </summary>
        /// <param name="context">Le contexte de base de données.</param>
        public SubscriptionRepository(GestionFormationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère tous les abonnements.
        /// </summary>
        /// <returns>Liste des abonnements.</returns>
        public async Task<IEnumerable<Subscription>> GetAllAsync() => await _context.Subscriptions.ToListAsync();

        /// <summary>
        /// Récupère un abonnement par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement.</param>
        /// <returns>L'abonnement ou null.</returns>
        public async Task<Subscription?> GetByIdAsync(int id) => await _context.Subscriptions.FindAsync(id);

        /// <summary>
        /// Ajoute un nouvel abonnement.
        /// </summary>
        /// <param name="entity">L'abonnement à ajouter.</param>
        public async Task AddAsync(Subscription entity)
        {
            _context.Subscriptions.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour un abonnement existant.
        /// </summary>
        /// <param name="entity">L'abonnement à mettre à jour.</param>
        public async Task UpdateAsync(Subscription entity)
        {
            _context.Subscriptions.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime un abonnement par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement à supprimer.</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Subscriptions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Récupère tous les abonnements d'un utilisateur spécifique.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <returns>Liste des abonnements de l'utilisateur.</returns>
        public async Task<IEnumerable<Subscription>> GetByUserIdAsync(int userId)
        {
            return await _context.Subscriptions
                .Include(s => s.Session)
                .ThenInclude(s => s.Formation)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère tous les abonnements pour une session spécifique.
        /// </summary>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>Liste des abonnements de la session.</returns>
        public async Task<IEnumerable<Subscription>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.Subscriptions
                .Include(s => s.User)
                .Where(s => s.SessionId == sessionId)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère un abonnement spécifique par utilisateur et session.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        /// <returns>L'abonnement ou null.</returns>
        public async Task<Subscription?> GetByUserAndSessionAsync(int userId, int sessionId)
        {
            return await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.SessionId == sessionId);
        }

        /// <summary>
        /// Supprime un abonnement par sa clé composite (utilisateur + session).
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="sessionId">Identifiant de la session.</param>
        public async Task DeleteByCompositeKeyAsync(int userId, int sessionId)
        {
            var entity = await GetByUserAndSessionAsync(userId, sessionId);
            if (entity != null)
            {
                _context.Subscriptions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
