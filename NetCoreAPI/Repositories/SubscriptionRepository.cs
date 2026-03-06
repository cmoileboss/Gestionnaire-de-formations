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
    }
}
