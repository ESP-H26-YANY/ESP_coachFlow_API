# Guide de Déploiement - CoachFlow API

## 1. Prérequis sur le Serveur

### Installation des dépendances système
```bash
# Sur Ubuntu/Debian
sudo apt update
sudo apt install -y wget curl gnupg2 lsb-release ubuntu-keyring

# Sur CentOS/RHEL
sudo yum update -y
```

### Installer .NET 8.0 SDK
```bash
# Ubuntu/Debian
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
CREATE DATABASE CoachFlowDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'coachflow'@'localhost' IDENTIFIED BY '1';
GRANT ALL PRIVILEGES ON CoachFlowDb.* TO 'coachflow'@'localhost';
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
  "Secret": "2bb6bf89d550eece354a8815ed1500a1889377b7" 
},
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=CoachFlowDb;User=root;Password=1;"
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

### Installer EF Core CLI (si pas déjà installé)
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

## 5. Configurer Systemd Service (Linux)

Crée `/etc/systemd/system/coachflow-api.service`:

```ini
[Unit]
Description=CoachFlow API
After=network.target mariadb.service

[Service]
Type=notify
User=www-data
WorkingDirectory=/var/www/coachflow
ExecStart=/usr/bin/dotnet /var/www/coachflow/publish/CoachFlowApi.Api.dll
Restart=on-failure
RestartSec=10

[Install]
WantedBy=multi-user.target
```

### Activer le service
```bash
sudo systemctl daemon-reload
sudo systemctl enable coachflow-api
sudo systemctl start coachflow-api
sudo systemctl status coachflow-api
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
# Installer Certbot
sudo apt install -y certbot python3-certbot-nginx

# Générer le certificat
sudo certbot certonly --nginx -d api.tondomaine.com

# Nginx va être configuré automatiquement
sudo systemctl restart nginx
```

---

## 8. Configuration de la Base de Données en Production

### Backup automatique
```bash
# Crée un script de backup: /usr/local/bin/backup-coachflow.sh
#!/bin/bash
BACKUP_DIR="/backups/coachflow"
mkdir -p $BACKUP_DIR
mysqldump -u coachflow -pton_password_fort CoachFlowDb > $BACKUP_DIR/backup_$(date +%Y%m%d_%H%M%S).sql
find $BACKUP_DIR -name "*.sql" -mtime +7 -delete  # Garde 7 jours de backups

# Ajoute au crontab (backup chaque jour à 2h du matin)
sudo crontab -e
# Ajoute: 0 2 * * * /usr/local/bin/backup-coachflow.sh
```

---

## 9. Vérifications Post-Déploiement

```bash
# Vérifier les logs
sudo journalctl -u coachflow-api -f

# Tester l'API
curl http://localhost:5000/swagger/index.html

# Vérifier la base de données
mysql -u coachflow -p -e "SELECT * FROM CoachFlowDb.Coaches LIMIT 5;"

# Vérifier l'espace disque
df -h

# Vérifier la mémoire
free -h
```

---

## 10. Checkliste de Déploiement

- [ ] .NET 8.0 SDK installé et dans le PATH
- [ ] MariaDB installé et démarré
- [ ] Base de données `CoachFlowDb` créée
- [ ] Utilisateur MariaDB créé avec permissions
- [ ] `appsettings.json` configuré avec les bons identifiants
- [ ] Migrations appliquées avec `dotnet ef database update`
- [ ] Build Release générée: `dotnet publish -c Release`
- [ ] Systemd service créé et activé
- [ ] Nginx configuré comme reverse proxy
- [ ] SSL/HTTPS configuré avec Let's Encrypt
- [ ] Firewall configuré (ports 80, 443 ouverts)
- [ ] Backups automatisés configurés
- [ ] Logs vérifiés et application en running

---

## 11. Commandes Utiles en Production

```bash
# Redémarrer l'API
sudo systemctl restart coachflow-api

# Voir les logs temps réel
sudo journalctl -u coachflow-api -f

# Arrêter l'API
sudo systemctl stop coachflow-api

# Voir le statut
sudo systemctl status coachflow-api

# Vérifier si le port 5000 est ouvert
netstat -tulpn | grep 5000
```

---


### Permission refusée
```bash
# Donner les permissions au dossier
sudo chown -R www-data:www-data /var/www/coachflow
sudo chmod -R 755 /var/www/coachflow
```

---

## Résumé des Étapes Essentielles

1. **Installer**: .NET 8.0 + MariaDB
2. **Configurer**: Base de données + `appsettings.json`
3. **Migrer**: `dotnet ef database update`
4. **Publier**: `dotnet publish -c Release`
5. **Déployer**: Copier les fichiers + Créer le service
6. **Sécuriser**: Nginx + SSL
7. **Monitorer**: Logs + Backups

Vous êtes prêt à déployer! 
