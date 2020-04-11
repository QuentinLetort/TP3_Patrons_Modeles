# TP3 Patrons Modèles

## Utilisation

1. Générer le projet en utilisant `dotnet build` depuis le répertoire racine.
    - Le projet AppWithPlugin ne contient aucune référence aux projets de plug-in, vous devez donc générer l'ensemble des projets.
2. Depuis le repertoire AppWithPlugin, utiliser `dotnet run LOADERNAME DIRECTORY` pour exécuter l'application.
    - Vous devriez voir un échantillon des données chargées.

## Réponses aux questions

#### 1. Comment organisez votre solution? (Combien de projets? Quels types de projets?)

2 + n projets dans la solution: 1 pour l'application, 1 pour l'interface entre l'application et les plugins et n pour les plugins de chargement de données.
Chaque plugin est un nouveau projet (JsonLoader et CsvLoader sont deux projets distincts).

#### 2. Dans quel projet mettez-vous la class User? Celle-ci est composée d’un nom, d’un prénom et d’une adresse mail.

La classe User est placée dans le projet d'application. En effet, nous avons choisi de développer un plugin de chargement de données génériques et donc non dépendant de la classe User.
Dans le cas où les plugins seraient dépendants de cette classe User il serait d'usage de la placer dans le projet d'interface application-plugins.

#### 3. En vous basant sur la documentation, à quoi ressemble l’interface d’un plugin dans notre système? (N’oubliez pas qu'il doit pouvoir charger une liste d’utilisateurs)

L'interface de DataLoadPlugin est simple, elle comporte deux propriétés sur le plugin (nom et description) ainsi qu'une méthode générique LoadData pour le chargement des données.
