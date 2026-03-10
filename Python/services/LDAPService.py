from ldap3 import Server, Connection, ALL, SUBTREE, Tls
from ldap3.core.exceptions import LDAPException
import ssl
import getpass

from dotenv import load_dotenv
import os

load_dotenv()
LDAP_SERVER = os.getenv("LDAP_SERVER")
LDAP_PASSWORD = os.getenv("LDAP_PASSWORD")
LDAP_USER = os.getenv("LDAP_USER")
LDAP_DOMAIN = os.getenv("LDAP_DOMAIN")

class LDAPService:

    def __init__(self, port=636):
        # Configuration TLS : chiffrement activé mais sans validation de certificat
        # ATTENTION : Vulnérable aux attaques MITM - uniquement pour dev/test
        tls_config = Tls(
            validate=ssl.CERT_NONE,        # Pas de validation du certificat
            version=ssl.PROTOCOL_TLS,      # Protocole TLS générique
            ca_certs_file=None,            # Pas de fichier CA
            valid_names=None,              # Pas de validation du hostname
            ciphers=None                   # Utiliser les ciphers par défaut
        )

        self.server = Server(
            LDAP_SERVER,
            port=port,
            use_ssl=False,
            get_info=ALL
        )
        
        if port == 636:
            self.server.use_ssl = True
            self.server.tls = tls_config
 

        # Format UPN : username@domain.com (format qui fonctionne avec Active Directory)
        self.patrice = f"{LDAP_USER}@{LDAP_DOMAIN}"


    def authenticate(self, username: str, password: str) -> bool:
        """Vérifie les identifiants stockés sur le serveur LDAP."""
        try:
            # Utiliser le format UPN pour l'authentification
            user_upn = f"{username}@{LDAP_DOMAIN}" if "@" not in username else username
    
            conn = Connection(
                self.server,
                user=user_upn,
                password=password,
                auto_bind=True
            )
            print(f"Authentification réussie pour {user_upn}")
            conn.unbind()
            print(f"Dissociation de l'utilisateur {user_upn}")
            return True
        except LDAPException as e:
            print("❌ Auth LDAP échouée:", e)
            return False
        except Exception as e:
            print(f"Authentification LDAP a échoué pour l'utilisateur {username}: {e}")
            return False