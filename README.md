# Gestionnaire de Formations - Documentation Projet

## Vue d'ensemble

Le **Gestionnaire de Formations** est une application web complète permettant la gestion centralisée des formations, sessions, évaluations et inscriptions des utilisateurs. Le projet propose deux implémentations backend distinctes offrant les mêmes fonctionnalités :

- **API .NET Core 10** (C#)
- **API Python FastAPI**

## Architecture Technique

### Stack Technologique

#### Back-end .NET Core
- **Framework** : ASP.NET Core 10.0 (preview .NET 10)
- **Base de données** : SQL Server avec Entity Framework Core 10.0.3
- **Authentification** : JWT Bearer tokens + BCrypt.Net-Next 4.1.0
- **Mapping** : AutoMapper 12.0.1
- **LDAP** : System.DirectoryServices.Protocols 10.0.0
- **Pattern** : Architecture en couches (Controllers → Services → Repositories → Models)

#### Back-end Python
- **Framework** : FastAPI
- **Base de données** : SQL Server avec SQLAlchemy
- **Authentification** : JWT + Passlib/BCrypt
- **LDAP** : ldap3
- **Pattern** : Architecture en couches (Routers → Services → Repositories → Models)

### Modèle de Données

L'application gère les entités suivantes :

1. **User** - Utilisateurs du système
2. **Formation** - Programmes de formation
3. **Module** - Modules pédagogiques composant les formations
4. **Session** - Sessions de formation planifiées
5. **Evaluation** - Évaluations des compétences
6. **Subscription** - Inscriptions des utilisateurs aux sessions
7. **Result** - Résultats des évaluations
8. **AIRecommandation** - Recommandations générées par IA (future fonctionnalité)

### Architecture en Couches

```
┌─────────────────────────────────────┐
│  Controllers/Routers (API REST)     │  ← Endpoints HTTP
├─────────────────────────────────────┤
│  DTOs/Request/Response Models       │  ← Validation & Serialization
├─────────────────────────────────────┤
│  Services (Business Logic)          │  ← Logique métier
├─────────────────────────────────────┤
│  Repositories (Data Access)         │  ← Accès aux données
├─────────────────────────────────────┤
│  Models (Entities)                  │  ← Modèles de domaine
├─────────────────────────────────────┤
│  Database (SQL Server)              │  ← Persistance
└─────────────────────────────────────┘
```

## Fonctionnalités Principales

### Gestion des Utilisateurs
- ✅ Inscription et authentification (email/password)
- ✅ Authentification LDAP/Active Directory
- ✅ Tokens JWT avec cookies HTTP-only
- ✅ Autorisation basée sur l'identité

### Gestion des Formations
- ✅ Création et organisation des formations
- ✅ Association de modules aux formations
- ✅ Gestion des descriptions et métadonnées
- ✅ CRUD complet sur les formations

### Gestion des Modules
- ✅ Création de modules pédagogiques
- ✅ Organisation par sujet et description
- ✅ Réutilisation dans plusieurs formations
- ✅ CRUD complet sur les modules

### Gestion des Sessions
- ✅ Planification de sessions de formation
- ✅ Gestion des dates, lieux et capacités
- ✅ Lien avec les formations
- ✅ Liste des participants inscrits

### Système d'Inscription
- ✅ Inscription des utilisateurs aux sessions
- ✅ Désinscription
- ✅ Suivi des dates d'inscription
- ✅ Consultation des sessions par utilisateur

### Évaluations et Résultats
- ✅ Création d'évaluations liées aux modules
- ✅ Enregistrement des résultats (score, succès/échec)
- ✅ Suivi des performances utilisateurs
- ✅ Horodatage des évaluations

## Bonnes Pratiques Implémentées

### Gestion d'Erreurs (.NET Core)

Le projet utilise le **pattern Result** pour une gestion d'erreurs robuste et performante :

```csharp
// Pattern Result<T> pour éviter les exceptions coûteuses
public async Task<Result<UserDto>> GetByIdAsync(int id)
{
    try
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return Result<UserDto>.Failure($"User {id} not found");
        return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("Database error", ex);
    }
}
```

**Avantages** :
- ✅ Performance optimale (évite le coût des exceptions)
- ✅ Séparation claire entre erreurs métier et erreurs système
- ✅ Type de retour explicite documentant les erreurs possibles
- ✅ Middleware global pour les erreurs imprévues

### Gestion d'Erreurs (Python)

Le projet suit la **philosophie EAFP** (Easier to Ask for Forgiveness than Permission) de Python :

```python
# Exceptions personnalisées pour la logique métier
def get_user(self, user_id: int) -> User:
    user = self.user_repository.get_by_id(user_id)
    if user is None:
        raise NotFoundError("Utilisateur", user_id)
    return user
```

**Exceptions personnalisées** :
- `NotFoundError` → HTTP 404
- `DuplicateError` → HTTP 409
- `UnauthorizedError` → HTTP 401
- `ForbiddenError` → HTTP 403
- `ValidationError` → HTTP 400

**Handlers globaux** pour convertir automatiquement les exceptions en réponses HTTP appropriées.

### Sécurité

#### Authentification Multi-Modes
1. **JWT Bearer** : Tokens stockés dans cookies HTTP-only sécurisés
2. **LDAP/Active Directory** : Intégration d'entreprise pour SSO
3. **Hachage BCrypt** : Mots de passe sécurisés avec salt

#### Autorisations
- Protection des endpoints par authentification requise
- Validation de l'identité (un utilisateur ne peut modifier que ses propres données)
- Gestion fine des droits d'accès via exceptions `ForbiddenError`

### Validation des Données

#### .NET Core
- **DTOs dédiés** pour création/mise à jour vs lecture
- **Annotations de validation** sur les modèles
- **AutoMapper** pour séparer les modèles du domaine et l'API

#### Python
- **Pydantic models** pour validation automatique
- **Request/Response models** séparés
- **Validation au niveau FastAPI** avec messages d'erreur explicites

## Structure des Projets

### .NET Core API

```
NetCoreAPI/
├── Controllers/          # API REST endpoints
├── Services/            # Logique métier
│   └── Interfaces/      # Contrats de services
├── Repositories/        # Accès aux données
│   └── Interfaces/      # Contrats de repositories
├── Models/              # Entités du domaine
├── DTOs/                # Data Transfer Objects
├── Data/                # DbContext EF Core
├── Utils/               # Utilitaires (Result<T>)
├── Migrations/          # Migrations Entity Framework
└── Program.cs           # Point d'entrée & configuration
```

### Python API

```
Python/
├── routers/                # API REST endpoints
├── services/               # Logique métier
├── repositories/           # Accès aux données
├── models/                 # Entités SQLAlchemy
├── apirequests/            # Modèles de requêtes
├── apiresponses/           # Modèles de réponses
├── exceptions.py           # Exceptions personnalisées
├── error_handlers.py       # Handlers d'exceptions
├── database_connection.py  # Connexion à la base de données SQL Server 2025
└── main.py                 # Point d'entrée FastAPI
```

## Configuration Requise

### Variables d'Environnement

#### .NET Core
```
ConnectionStrings__DefaultConnection=Server=...;Database=GestionFormation;...
Jwt__Key=<secret_key>
Jwt__Issuer=api.formation.com
Jwt__Audience=api.formation.com
LDAP_SERVER=ldap.entreprise.local
LDAP_DOMAIN=ENTREPRISE
```

#### Python
```
DATABASE_URL=mssql+pyodbc://...
JWT_SECRET_KEY=<secret_key>
JWT_ALGORITHM=HS256
LDAP_SERVER=ldap.entreprise.local
LDAP_DOMAIN=ENTREPRISE
```

### Prérequis
- SQL Server 2025
- .NET Runtime 10.0+ (pour API C#)
- Python 3.11+ (pour API Python)
- Active Directory/LDAP (optionnel, pour authentification entreprise)

## API REST - Endpoints Complets

### Authentification
- `POST /login` - Connexion avec email/mot de passe (génère JWT)
- `POST /register` - Inscription d'un nouvel utilisateur
- `POST /logout` - Déconnexion (supprime le cookie JWT)
- `POST /ldap` - Authentification via LDAP/Active Directory (génère JWT)

### Utilisateurs
- `GET /users` - Liste de tous les utilisateurs
- `GET /users/{id}` - Détails d'un utilisateur spécifique
- `PUT /users/{id}` - Mise à jour du profil utilisateur
- `DELETE /users/{id}` - Suppression du compte utilisateur
- `GET /users/{id}/sessions` - Sessions auxquelles l'utilisateur est inscrit
- `POST /users/{id}/sessions/{sessionId}` - Inscrire l'utilisateur à une session
- `DELETE /users/{id}/sessions/{sessionId}` - Désinscrire l'utilisateur d'une session
- `GET /users/{id}/evaluations` - Évaluations auxquelles l'utilisateur est inscrit
- `POST /users/{id}/evaluations/{evaluationId}` - Inscrire l'utilisateur à une évaluation

### Formations
- `GET /formations` - Liste de toutes les formations
- `GET /formations/{id}` - Détails d'une formation
- `POST /formations` - Créer une nouvelle formation
- `PUT /formations/{id}` - Modifier une formation
- `DELETE /formations/{id}` - Supprimer une formation
- `GET /formations/{id}/modules` - Modules associés à la formation
- `POST /formations/{id}/modules/{moduleId}` - Associer un module à la formation
- `DELETE /formations/{id}/modules/{moduleId}` - Retirer un module de la formation

### Modules
- `GET /modules` - Liste de tous les modules
- `GET /modules/{id}` - Détails d'un module
- `POST /modules` - Créer un nouveau module
- `PUT /modules/{id}` - Modifier un module
- `DELETE /modules/{id}` - Supprimer un module

### Sessions
- `GET /sessions` - Liste de toutes les sessions
- `GET /sessions/{id}` - Détails d'une session
- `POST /sessions` - Créer une nouvelle session
- `PUT /sessions/{id}` - Modifier une session
- `DELETE /sessions/{id}` - Supprimer une session
- `GET /sessions/{id}/users` - Liste des utilisateurs inscrits à la session

### Abonnements (Subscriptions)
- `GET /subscriptions` - Liste de tous les abonnements
- `GET /subscriptions/{id}` - Détails d'un abonnement par ID unique
- `POST /subscriptions` - Créer un nouvel abonnement
- `PUT /subscriptions/{id}` - Modifier un abonnement
- `DELETE /subscriptions/{id}` - Supprimer un abonnement par ID
- `GET /subscriptions/user/{userId}` - Abonnements d'un utilisateur spécifique
- `GET /subscriptions/session/{sessionId}` - Abonnements pour une session spécifique
- `GET /subscriptions/{userId}/{sessionId}` - Abonnement par clé composite
- `DELETE /subscriptions/{userId}/{sessionId}` - Supprimer un abonnement par clé composite

### Évaluations
- `GET /evaluations` - Liste de toutes les évaluations
- `GET /evaluations/{id}` - Détails d'une évaluation
- `POST /evaluations` - Créer une nouvelle évaluation
- `PUT /evaluations/{id}` - Modifier une évaluation
- `DELETE /evaluations/{id}` - Supprimer une évaluation
- `GET /evaluations/{id}/users` - Liste des utilisateurs inscrits à l'évaluation

### Résultats
- `GET /results` - Liste de tous les résultats
- `GET /results/{userId}/{evaluationId}` - Résultat par clé composite
- `POST /results` - Enregistrer un nouveau résultat
- `PUT /results/{userId}/{evaluationId}` - Modifier un résultat
- `DELETE /results/{userId}/{evaluationId}` - Supprimer un résultat

## Documentation API

Les deux implémentations exposent une documentation interactive :

### .NET Core
- **Swagger UI** : `https://localhost:5001/swagger`
- **Spécification OpenAPI (utilisable avec Postman)** : `https://localhost:5001/swagger/v1/swagger.json`

### Python FastAPI
- **Swagger UI** : `http://localhost:8000/docs`
- **ReDoc** : `http://localhost:8000/redoc`
- **Spécification OpenAPI (utilisable avec Postman)** : `http://localhost:8000/openapi.json`

### Import dans Postman

Pour importer automatiquement tous les endpoints dans Postman :

1. Démarrez l'API (`.NET` ou `Python`)
2. Dans Postman, cliquez sur **Import**
3. Sélectionnez **Link** et collez l'URL de la spécification :
   - .NET Core : `https://localhost:5001/swagger/v1/swagger.json`
   - Python : `http://localhost:8000/openapi.json`
4. Cliquez sur **Import**

Postman créera automatiquement une collection complète avec tous les endpoints, paramètres et schémas de requête.

## Performances et Scalabilité

### .NET Core
- ✅ Pattern Result évitant les exceptions coûteuses
- ✅ Async/await pour opérations I/O
- ✅ Entity Framework Core avec tracking optimisé
- ✅ Compilation AOT possible (.NET 10)

### Python
- ✅ FastAPI asynchrone pour haute concurrence
- ✅ Exceptions légères (overhead faible en Python)
- ✅ SQLAlchemy avec query optimization
- ✅ Pydantic pour validation ultra-rapide

## Tests et Quality Assurance

### Validation
- ✅ Aucune erreur de compilation (.NET)
- ✅ Aucune erreur de linting (Python)
- ✅ Schémas de validation cohérents
- ✅ Gestion d'erreurs complète et robuste

### Sécurité
- ✅ Hachage BCrypt pour mots de passe
- ✅ Tokens JWT sécurisés
- ✅ Cookies HTTP-only
- ✅ Validation des entrées utilisateur
- ✅ Protection CSRF via SameSite cookies
- ✅ Séparation des erreurs métier/système

## Points Forts du Projet

1. **Double Implémentation** : Flexibilité technologique (C# ou Python selon besoins)
2. **Architecture Solide** : Séparation claire des responsabilités
3. **Sécurité Renforcée** : Multi-modes d'authentification, tokens sécurisés
4. **Maintenabilité** : Code structuré, bonnes pratiques, gestion d'erreurs cohérente
5. **Performance** : Pattern optimisés pour chaque langage
6. **Extensibilité** : Architecture prête pour IA (recommandations), rapports, notifications
7. **Documentation** : APIs documentées automatiquement (Swagger/OpenAPI)
8. **Prêt pour la Production** : Gestion d'erreurs robuste, logging, middleware global

## Évolutions Futures Possibles

### Fonctionnalités Métier
- 📊 Tableau de bord et statistiques
- 🤖 Recommandations IA basées sur le profil utilisateur
- 📧 Notifications par email (inscriptions, rappels)
- 📅 Export calendrier (iCal)
- 📄 Génération de certificats PDF
- 🎓 Badges et gamification

### Technique
- 🔄 Synchronisation temps réel (SignalR / WebSockets)
- 📱 Application mobile (API prête)
- 🌍 Internationalisation (i18n)
- 📈 Monitoring et observabilité (Application Insights / Prometheus)
- 🧪 Tests unitaires et d'intégration
- 🐳 Containerisation Docker
- ☁️ Déploiement cloud (Azure / AWS)

## Conclusion

Le **Gestionnaire de Formations** est une solution complète, sécurisée et performante pour la gestion des formations en entreprise. Les deux implémentations (C# et Python) suivent les meilleures pratiques de leur écosystème respectif, garantissant maintenabilité et évolutivité du projet.

Le système est prêt pour un déploiement en production et peut facilement être étendu avec de nouvelles fonctionnalités selon les besoins du client.

---

**Date de livraison** : Mars 2026  
**Versions** : .NET Core 10.0 / Python 3.11 + FastAPI  
**Base de données** : SQL Server 2025
**Status** : ✅ Production-ready