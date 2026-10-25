#ADAMOU HAMADOU TIZE 22G00417 BiblioCampus

Application .NET MAUI (.NET 10) de gestion et de consultation d'une bibliothèque universitaire.

## Fonctionnalités
- Catalogue avec recherche (titre, auteur, ISBN, catégorie) et disponibilité
- Emprunt d'un livre (durée : 14 jours), suivi des emprunts en cours / en retard
- Retour d'un livre (mise à jour automatique du stock)
- Ajout de livres au catalogue (espace bibliothécaire)

## Architecture
MVVM (CommunityToolkit.Mvvm) · SQLite local (sqlite-net-pcl) · Injection de dépendances · Shell

## Prérequis
SDK .NET 10 + workload MAUI : `dotnet workload install maui`

## Lancer
    dotnet build -t:Run -f net10.0-android
    dotnet run -f net10.0-windows10.0.19041.0   (sous Windows)

iOS / macOS : ajouter `net10.0-ios;net10.0-maccatalyst` dans le .csproj et les dossiers Platforms correspondants (sur un Mac).
