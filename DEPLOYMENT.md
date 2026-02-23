# Guide de Déploiement - CoachFlow API

Ce document détaille les étapes pour déployer l'API CoachFlow sur un serveur Linux (Ubuntu/Debian), MariaDB comme base de données.

---


### 1.Installation des dépendances système
Mise à jour des paquets et installation des outils de base :

```bash
sudo apt update
sudo apt install -y wget apt-transport-https
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-8.0
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
CREATE DATABASE coachflowdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

```
# Préparer le Projet
```bash
cd /ESP_coachFlow_Front

dotnet restore
dotnet build

dotnet publish CoachFlowApi.Api -c Release -o publish
```

# Mettre à jour appsettings.json et migration
- Ajoutez le fichier appsettings.json dans CoachFlowApi/CoachFlowApi.Api et mettez vos informations dedans.

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