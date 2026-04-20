# CoachFlow API 

## Présentation
CoachFlow est le backend d'une plateforme web de coaching sportif. L'application permet aux coachs de publier et vendre des guides d'entraînement numériques (PDF), et aux élèves de les acheter, de les sauvegarder dans leur bibliothèque personnelle et de les consulter. 

Le projet met l'accent sur une architecture modulaire 'CLean Architecture', la sécurité applicative (JWT, hachage) et le traitement automatisé des fichiers.

##  Fonctionnalités Principales
* **Authentification & Sécurité :** Inscription, connexion, hachage des mots de passe avec BCrypt et sécurisation des routes via JWT.
* **Gestion des rôles :** Séparation stricte des permissions entre les `coachs` et les `users` (élèves).
* **Gestion des Guides :** * Création de guides par les coachs avec upload de fichiers PDF.
  * Traitement d'image intégré (Magick.NET) pour extraire automatiquement la première page du PDF et générer une image de couverture (JPG).
  * Opérations CRUD complètes sur les guides.
* **Bibliothèque Utilisateur :** Système permettant aux élèves d'ajouter et de retirer des guides de leur espace personnel (SavedGuides).
* **Documentation API :** Interface Swagger intégrée pour tester facilement les points de terminaison.
## Technologies Utilisées
* **Framework :** .NET 8.0 (C#)
* **Base de données :** MariaDB 
* **ORM :** Entity Framework Core 
* **Architecture :** Clean Architecture 
* **Sécurité :** JSON Web Tokens (JWT), BCrypt.Net-Next
* **Validation :** FluentValidation
* **Traitement de fichiers :** Magick.NET 

##  Architecture du Projet
Le projet suit les principes de la **Clean Architecture**, divisé en 4 couches distinctes pour garantir la maintenabilité et l'évolutivité :

1. **`CoachFlowApi.Domain`** : Le cœur du système. Contient les entités métier (`User`, `Coach`, `Guide`, `Purchase`, `SavedGuide`) et les interfaces des dépôts (Repositories). Ne dépend d'aucune autre couche.
2. **`CoachFlowApi.Application`** : Contient la logique métier via le pattern *Use Cases*. On y trouve les DTOs (Data Transfer Objects) et les règles de validation (FluentValidation).
3. **`CoachFlowApi.Infrastructure`** : Gère l'accès aux données. Contient la configuration d'Entity Framework (`AppDbContext`), l'implémentation des repositories et les migrations de la base de données.
4. **`CoachFlowApi.Api`** : La couche de présentation. Contient les contrôleurs REST (`AuthController`, `GuideController`, `LibraryController`), l'injection de dépendances et la configuration de l'application (`Program.cs`).

##  Prérequis pour le développement
Pour faire tourner ce projet en local, vous avez besoin de :
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Un serveur MariaDB en cours d'exécution.
* [Ghostscript](https://ghostscript.com/) installé sur votre machine (indispensable pour que Magick.NET puisse lire les PDF).

# Structure du projet
```
ESP_coachFlow_API/
├── CoachFlowApi.sln                     # Fichier de solution principal
├── CoachFlowApi.Api/                    # Couche Présentation (Contrôleurs REST)
│   ├── Controllers/                     # AuthController, GuideController, LibraryController
│   ├── Properties/                      # launchSettings.json
│   ├── wwwroot/                         # Dossier racine pour les fichiers statiques (images, pdfs)
│   ├── appsettings.json                 # Configuration (BDD, JWT, Kestrel)
│   └── Program.cs                       # Point d'entrée de l'application
├── CoachFlowApi.Application/            # Couche Logique Métier (Cas d'utilisation)
│   ├── DTOs/                            # Objets de transfert de données (AuthDto, GuideDto, etc.)
│   ├── UseCases/                        # Implémentation de la logique métier (User, Guide, Library)
│   ├── Validators/                      # Règles de validation (FluentValidation)
│   └── DependencyInjection.cs           # Enregistrement des services applicatifs
├── CoachFlowApi.Domain/                 # Couche Domaine (Noyau métier)
│   ├── Entities/                        # Entités (User, Coach, Guide, Purchase, SavedGuide)
│   └── Interfaces/                      # Interfaces des dépôts (Repositories)
├── CoachFlowApi.Infrastructure/         # Couche Infrastructure (Accès aux données)
│   ├── Data/                            # AppDbContext et configurations Entity Framework
│   ├── Migrations/                      # Historique des migrations de la base de données
│   ├── Repositories/                    # Implémentation concrète des interfaces du domaine
│   └── DependencyInjection.cs           # Configuration de la BDD et injection d'infrastructure
└── CoachFlowApi.Tests/                  # Projets de tests unitaires (xUnit, Moq)
```

Pour les instructions de mise en production (Serveur Linux, PM2), veuillez consulter le fichier DEPLOYMENT.md

Modifiez le fichier **CoachFlowApi.Api/appsettings.json** pour y insérer vos identifiants MariaDB et votre clé secrète JWT.

**Auteur : Yany Boudedja**
