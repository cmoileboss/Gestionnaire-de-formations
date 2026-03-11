using System.DirectoryServices.Protocols;
using System.Net;

using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des authentifications au serveur LDAP.
/// Les informations importantes sont contenus dans le fichier .env.
/// </summary>
public class LDAPService
{
    private readonly string _server;
    private readonly int _port;
    private readonly string _domain;
    private readonly IConfiguration _config;

    public LDAPService(IConfiguration config)
    {
        this._config = config;
        this._server = Environment.GetEnvironmentVariable("LDAP_SERVER") ?? throw new InvalidOperationException("LDAP_SERVER environment variable is not set");
        this._port = 389;
        this._domain = Environment.GetEnvironmentVariable("LDAP_DOMAIN") ?? throw new InvalidOperationException("LDAP_DOMAIN environment variable is not set");
    }

    public string? Authenticate(string username, string password)
    {
        var identifier = new LdapDirectoryIdentifier(_server, _port);
            
        // Format UPN : username@domain.com
        var userPrincipalName = username.Contains("@") 
            ? username 
            : $"{username}@{_domain}";
        
        // Créer les credentials
        var credentials = new NetworkCredential(userPrincipalName, password);
        
        // Créer la connexion LDAP
        try
        {
            var connection = new LdapConnection(identifier, credentials)
            {
                // AuthType selon votre AD : Basic, Negotiate, Kerberos
                AuthType = AuthType.Basic
            };

            // Bind (authentification)
            connection.Bind();
            
            // Si on arrive ici, l'authentification a réussi
            var token = TokenManager.CreateLDAPToken(username, _config);
            return token;
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"LDAP authentication failed for user {username}: {ex.Message}");   
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Authentification LDAP a échoué pour l'utilisateur {username}: {e}");
            return null;
        }
    }
}