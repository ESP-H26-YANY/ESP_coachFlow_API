# Guide de Déploiement - CoachFlow API

## 1. Prérequis sur le Serveur

### Installation des dépendances système
```bash
# Sur Ubuntu/Debian
sudo apt update
sudo apt install -y wget curl gnupg2 lsb-release ubuntu-keyring

```

### Installer .NET 8.0 SDK
```bash

wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --version 8.0.0
export PATH=$PATH:$HOME/.dotnet
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc

# Vérifier l'installation
dotnet --version
```

### Installer MariaDB Server
```bash
# Ubuntu/Debian
sudo apt install -y mariadb-server mariadb-client

# Démarrer le service
sudo systemctl start mariadb
sudo systemctl enable mariadb

# Vérifier
sudo systemctl status mariadb
```

### Configurer MariaDB
```bash
# Se connecter à MariaDB
sudo mysql -u root

# Dans MySQL:
CREATE DATABASE coachflowdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'coachflow'@'localhost' IDENTIFIED BY '1';
GRANT ALL PRIVILEGES ON coachflowdb.* TO 'coachflow'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

---

## 2. Préparer le Projet

```bash
# Restaurer les dépendances
dotnet restore

# Build le projet
dotnet build
```

---

## 3. Configuration de la Base de Données

### Mettre à jour appsettings.json
Édite `CoachFlowApi.Api/appsettings.json`:

```json
{
  "Jwt": {
  "Issuer": "CoachFlowApi",
  "Audience": "CoachFlowClient",
  "key": "2bb6bf89d550eece354a8815ed1500a1889377b7" 
},
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=coachflowdb;User=root;Password=1;"
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

### Installer EF Core CLI 
```bash
dotnet tool install --global dotnet-ef --version 8.0.0
export PATH=$PATH:$HOME/.dotnet/tools
```

### Appliquer les Migrations
```bash
cd /path/to/CoachFlow_Api

# Appliquer toutes les migrations
$HOME/.dotnet/tools/dotnet-ef database update \
  --project CoachFlowApi.Infrastructure \
  --startup-project CoachFlowApi.Api
```

---

## 4. Publier l'Application

```bash
# Créer une build Release
dotnet publish -c Release -o ./publish

# Les fichiers compilés seront dans ./publish/
```

---



## 6. Configurer Nginx comme Reverse Proxy

Crée `/etc/nginx/sites-available/coachflow-api`:

```nginx
server {
    listen 80;
    server_name api.tondomaine.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

### Activer le site
```bash
sudo ln -s /etc/nginx/sites-available/coachflow-api /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx
```

---

## 7. SSL avec Let's Encrypt (HTTPS)

```bash
sudo apt install -y certbot python3-certbot-nginx

sudo certbot certonly --nginx -d api.tondomaine.com

sudo systemctl restart nginx
```
