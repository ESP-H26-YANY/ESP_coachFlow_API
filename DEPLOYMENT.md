## Installation des dépendances système
installer, configurer et lancer l'API CoachFlow sur un serveur Linux (Ubuntu) vierge. L'API tournera en arrière-plan via PM2. 
La base de données est mariaDB.

## Prérequis
```bash
# 1. Mise à jour complète du système
sudo apt update && sudo apt upgrade -y

# 2. Utilitaires de base
sudo apt install -y git curl wget nano unzip apt-transport-https

# 3. Installation de Ghostscript (OBLIGATOIRE pour Magick.NET - Conversion PDF -> JPG)
sudo apt install -y ghostscript

# 4. Installation de MariaDB (Base de données)
sudo apt install -y mariadb-server mariadb-client
sudo systemctl start mariadb
sudo systemctl enable mariadb

# 5. Installation de Node.js et PM2 (Pour garder l'API en ligne)
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo bash -
sudo apt install -y nodejs
sudo npm install -g pm2

# 6. Installation du SDK .NET 8.0
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-8.0
```


# Préparation de la base de données (MariaDB)
Toujours en root, connectez-vous à MariaDB pour créer la base de données et l'utilisateur associé.

```bash
sudo mysql -u root

CREATE DATABASE coachflowdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'coachflow'@'localhost' IDENTIFIED BY '1';
GRANT ALL PRIVILEGES ON coachflowdb.* TO 'coachflow'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

# Création de l'utilisateur de déploiement
```bash
# Créer l'utilisateur deploy
sudo adduser deploy

# Ajouter l'utilisateur au groupe sudo
sudo usermod -aG sudo deploy

# Préparer le répertoire d'hébergement
sudo mkdir -p /var/www
sudo chown -R deploy:deploy /var/www
sudo chmod -R 755 /var/www
```

# Clonage du projet
Basculez sur l'utilisateur deploy pour la suite des opérations.
```bash
su - deploy
cd /var/www

# Cloner le dépôt (remplacez l'URL par la vôtre)
git clone https://github.com/......lien 
cd ESP_coachFlow_API
```

# Configuration de l'environnement

L'API a besoin des clés JWT et des accès à la base de données. Créez ou modifiez le fichier appsettings.json dans le dossier de l'API.
``` bash
nano /var/www/ESP_coachFlow_API/CoachFlowApi.Api/appsettings.json
```
Contenu à insérer :
```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5144"
      }
    }
  },
  "Jwt": {
    "Issuer": "CoachFlowApi",
    "Audience": "CoachFlowClient",
    "Key": "VOTRE_CLE_SECRETE_TRES_LONGUE_QUI_DOIT_FAIRE_AU_MOINS_64_CARACTERES" 
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
# Migrations Entity Framework compilation et lancement avec PM2
```bash
# Installation de l'outil EF globalement
dotnet tool install --global dotnet-ef --version 8.0.0

# Ajout des outils dotnet au PATH pour pouvoir les utiliser
export PATH="$PATH:$HOME/.dotnet/tools"
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc

# Exécuter les migrations depuis la racine du projet
cd /var/www/ESP_coachFlow_API
dotnet ef database update --project ./CoachFlowApi.Infrastructure --startup-project ./CoachFlowApi.Api
```

- Compilation et Publication
```bash
cd /var/www/ESP_coachFlow_API

# Restauration des paquets NuGet
dotnet restore

# Compilation et création du dossier 'publish'
dotnet publish CoachFlowApi.Api/CoachFlowApi.Api.csproj -c Release -o ./publish

# Création manuelle du dossier des uploads pour éviter les erreurs de permissions avec IFormFile
mkdir -p ./publish/wwwroot/uploads/guides
```

- Lancement avec PM2
```bash 
# Se placer dans le dossier publié
cd /var/www/ESP_coachFlow_API/publish

# Démarrer le processus
pm2 start "dotnet CoachFlowApi.Api.dll" --name coachflow-api

# Sauvegarder la liste des processus PM2
pm2 save

# Générer le script de démarrage PM2
pm2 startup
```
**Important : La commande pm2 startup va afficher une commande dans le terminal (commençant par sudo env PATH...). Copiez et collez cette commande dans votre terminal pour que l'API démarre automatiquement au redémarrage du serveur.**

# Vérifications
```bash 
# Vérifier le statut PM2
pm2 status

# Consulter les logs de l'API (utile si ça crash)
pm2 logs coachflow-api

# Tester localement que l'API répond
curl -I http://localhost:5144/swagger/index.html
```

L'API tourne désormais parfaitement sur le port 5144 en local sur le serveur.