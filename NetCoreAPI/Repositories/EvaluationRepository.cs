using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    /// <summary>
    /// Repository pour la gestion des entités Evaluation.
    /// Fournit des méthodes CRUD pour les évaluations.
    /// </summary>
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly GestionFormationContext _context;
        /// <summary>
        /// Initialise une nouvelle instance du repository Evaluation.
        /// </summary>
        /// <param name="context">Contexte de base de données injecté.</param>
        public EvaluationRepository(GestionFormationContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Récupère toutes les évaluations.
        /// </summary>
        /// <returns>Liste des évaluations.</returns>
        public async Task<IEnumerable<Evaluation>> GetAllAsync() => await _context.Evaluations.ToListAsync();

        /// <summary>
        /// Récupère une évaluation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'évaluation.</param>
        /// <returns>Évaluation correspondante ou null.</returns>
        public async Task<Evaluation?> GetByIdAsync(int id) => await _context.Evaluations.FindAsync(id);

        /// <summary>
        /// Ajoute une nouvelle évaluation.
        /// </summary>
        /// <param name="entity">Évaluation à ajouter.</param>
        public async Task AddAsync(Evaluation entity)
        {
            _context.Evaluations.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour une évaluation existante.
        /// </summary>
        /// <param name="entity">Évaluation à mettre à jour.</param>
        public async Task UpdateAsync(Evaluation entity)
        {
            _context.Evaluations.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime une évaluation par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'évaluation à supprimer.</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Evaluations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
