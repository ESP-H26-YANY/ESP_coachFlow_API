# Guide de Déploiement - CoachFlow API

Ce document détaille les étapes pour déployer l'API CoachFlow sur un serveur Linux (Ubuntu/Debian) avec Nginx comme reverse proxy et MariaDB comme base de données.

---

## 1. Prérequis sur le Serveur

Connectez-vous à votre serveur via SSH.

### 1.1 Installation des dépendances système
Mise à jour des paquets et installation des outils de base :

```bash
sudo apt update
sudo apt install -y wget curl gnupg2 lsb-release ubuntu-keyring git
```
# Installer .NET 8.0 SDK
```
wget [https://dot.net/v1/dotnet-install.sh](https://dot.net/v1/dotnet-install.sh) -O dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --version 8.0.0
```

# Ajouter .NET au PATH pour la session actuelle et les futures
```bash
export PATH=$PATH:$HOME/.dotnet
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc
```

# Vérifier l'installation
dotnet --version

# Installer et Configurer MariaDB
```bash
sudo apt install -y mariadb-server mariadb-client

# Démarrer le service et l'activer au démarrage
sudo systemctl start mariadb
sudo systemctl enable mariadb

# Vérifier le statut
sudo systemctl status mariadb

# Se connecter à MariaDB en tant que root
sudo mysql -u root
```
```sql
-- Création de la base
CREATE DATABASE coachflowdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- Création de l'utilisateur (remplacez '1' par un mot de passe fort en production)
CREATE USER 'coachflow'@'localhost' IDENTIFIED BY '1';

-- Attribution des droits
GRANT ALL PRIVILEGES ON coachflowdb.* TO 'coachflow'@'localhost';
FLUSH PRIVILEGES;
EXIT;

```
# Préparer le Projet
```bash
# Se placer dans le dossier du projet
cd /path/to/CoachFlow_Api

# Restaurer les dépendances
dotnet restore

# Vérifier que le projet compile
dotnet build --no-restore
```

# Mettre à jour appsettings.json et migration
```json
{
  "Jwt": {
    "Issuer": "CoachFlowApi",
    "Audience": "CoachFlowClient",
    "Key": "2bb6bf89d550eece354a8815ed1500a1889377b7" 
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=coachflowdb;User=coachflow;Password=1;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
# migration
```bash
dotnet tool install --global dotnet-ef --version 8.0.0
export PATH=$PATH:$HOME/.dotnet/tools

# Assurez-vous d'être à la racine de la solution
dotnet ef database update \
  --project ./CoachFlowApi.Infrastructure \
  --startup-project ./CoachFlowApi.Api
```

- Publier l'Application
```bash 
# Publier dans le dossier local ./publish
dotnet publish CoachFlowApi.Api/CoachFlowApi.Api.csproj -c Release -o ./publish

# Déplacer les fichiers vers un dossier standard
sudo mkdir -p /var/www/coachflow
sudo cp -r ./publish/* /var/www/coachflow/

# Donner les droits appropriés
sudo chown -R www-data:www-data /var/www/coachflow
sudo chmod -R 755 /var/www/coachflow
```