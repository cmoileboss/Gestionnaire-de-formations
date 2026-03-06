using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Models;
using NetCoreAPI.Data;

namespace NetCoreAPI.Repositories;

/// <summary>
/// Repository pour la gestion des utilisateurs (accès base de données).
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly GestionFormationContext _context;

    /// <summary>
    /// Initialise une nouvelle instance du repository utilisateur.
    /// </summary>
    /// <param name="context">Le contexte de base de données.</param>
    public UserRepository(GestionFormationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Récupère tous les utilisateurs de la base de données.
    /// </summary>
    /// <returns>Une liste d'utilisateurs.</returns>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <summary>
    /// Récupère un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'utilisateur.</param>
    /// <returns>L'utilisateur ou null s'il n'existe pas.</returns>
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
    }

    /// <summary>
    /// Récupère un utilisateur par son email.
    /// </summary>
    /// <param name="email">L'email de l'utilisateur.</param>
    /// <returns>L'utilisateur ou null s'il n'existe pas.</returns>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="user">L'utilisateur à créer.</param>
    /// <returns>L'utilisateur créé.</returns>
    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Met à jour un utilisateur existant.
    /// </summary>
    /// <param name="user">L'utilisateur à mettre à jour.</param>
    /// <returns>L'utilisateur mis à jour.</returns>
    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Supprime un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'utilisateur à supprimer.</param>
    /// <returns>True si l'utilisateur a été supprimé, sinon false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Vérifie si un utilisateur existe par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'utilisateur.</param>
    /// <returns>True si l'utilisateur existe, sinon false.</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Users.AnyAsync(u => u.UserId == id);
    }

    /// <summary>
    /// Vérifie si un utilisateur existe par son email.
    /// </summary>
    /// <param name="email">L'email à vérifier.</param>
    /// <returns>True si l'email existe, sinon false.</returns>
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}