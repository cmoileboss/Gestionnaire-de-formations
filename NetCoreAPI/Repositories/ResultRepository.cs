using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Repository pour la gestion des entités Result.
    /// Fournit des méthodes CRUD pour les résultats d'évaluation (clé composite UserId + EvaluationId).
    /// </summary>
    public class ResultRepository : IResultRepository
    {
        private readonly GestionFormationContext _context;

        /// <summary>
        /// Initialise une nouvelle instance du repository Result.
        /// </summary>
        /// <param name="context">Contexte de base de données injecté.</param>
        public ResultRepository(GestionFormationContext context)
        {
            _context = context;
        }

        /// <summary>Récupère tous les résultats.</summary>
        public async Task<IEnumerable<Result>> GetAllAsync() =>
            await _context.Results.ToListAsync();

        /// <summary>Récupère un résultat par sa clé composite.</summary>
        public async Task<Result?> GetByIdAsync(int userId, int evaluationId) =>
            await _context.Results.FindAsync(userId, evaluationId);

        /// <summary>Ajoute un nouveau résultat.</summary>
        public async Task AddAsync(Result entity)
        {
            _context.Results.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>Met à jour un résultat existant.</summary>
        public async Task UpdateAsync(Result entity)
        {
            _context.Results.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>Supprime un résultat par sa clé composite.</summary>
        public async Task DeleteAsync(int userId, int evaluationId)
        {
            var entity = await GetByIdAsync(userId, evaluationId);
            if (entity != null)
            {
                _context.Results.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
