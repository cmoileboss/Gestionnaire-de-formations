namespace NetCoreAPI.Services
{
    public interface ILDAPService
    {
        /// <summary>
        /// Authentifie un utilisateur via LDAP.
        /// </summary>
        /// <param name="username">Nom d'utilisateur LDAP</param>
        /// <param name="password">Mot de passe LDAP</param>
        /// <returns>Token LDAP si authentifié, sinon null</returns>
        string? Authenticate(string username, string password);
    }
}
