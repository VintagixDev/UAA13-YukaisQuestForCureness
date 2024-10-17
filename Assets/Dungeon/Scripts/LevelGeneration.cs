using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    /// <summary>
    /// Contient les informations sur la taille du monde
    /// worldSize définit la taille de la grille dans laquelle les pièces seront placées
    /// takenPositions garde une trace des positions déjà occupées dans cette grille
    /// </summary>
    [Header("World data")]
    Vector2 worldSize = new Vector2(4, 4);
    List<Vector2> takenPositions = new List<Vector2>();

    /// <summary>
    /// Contient les informations relatives aux pièces
    /// rooms est un tableau bidimensionnel représentant toutes les pièces dans la grille
    /// numberOfRooms est le nombre total de pièces à générer
    /// roomWhiteObj est le prefab (objet) utilisé pour représenter les pièces dans la scène
    /// </summary>
    [Header("Rooms data")]
    Room[,] rooms;
    int numberOfRooms = 20;
    public GameObject roomWhiteObj;

    /// <summary>
    /// Stocke les dimensions de la grille utilisée pour générer le niveau
    /// gridSizeX et gridSizeY représentent respectivement la largeur et la hauteur de la grille
    /// </summary>
    [Header("Grid")]
    int gridSizeX;
    int gridSizeY;

    void Start()
    {
        // On s'assure qu'il n'y a pas plus de pièce que de place dans la grille
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms();
        SetRoomDoors();
        DrawMap();
    }

    /// <summary>
    /// Cette fonction crée les pièces dans une grille de taille gridSizeX * 2 par gridSizeY * 2
    /// Elle positionne chaque pièce aléatoirement en évitant de placer des pièces avec plus d'un voisin immédiat,
    /// et régule la probabilité de placement en fonction de la progression du nombre de pièces créées
    /// </summary>
    void CreateRooms()
    {
        // Initialisation de la grille avec la taille définie, multipliée par deux pour chaque dimension
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];

        // Placement de la première pièce (position centrale dans la grille) avec un type de pièce de valeur 1
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);

        // La position de départ est (0, 0), donc elle est ajoutée à la liste des positions occupées
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero; // Variable temporaire pour stocker les nouvelles positions

        // Variables pour réguler la probabilité de placement d'une pièce avec plusieurs voisins
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        // Boucle pour ajouter le nombre défini de pièces (20 pièces par défaut, moins 1 car la première pièce est déjà placée)
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            // Calcul du pourcentage de progression du placement des pièces
            float randomPerc = ((float)i) / ((float)numberOfRooms - 1);

            // Interpolation linéaire pour ajuster la probabilité de placement d'une pièce avec plusieurs voisins
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            // Obtenir une nouvelle position aléatoire pour une pièce
            checkPos = NewPosition();

            // Si la nouvelle position a plus d'un voisin ou si la probabilité aléatoire est supérieure à randomCompare :
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int j = 0; // Compteur pour limiter les tentatives

                // Cherche une nouvelle position tant qu'elle a trop de voisins et que le nombre de tentatives est inférieur à 100
                do
                {
                    checkPos = SelectiveNewPosition(); // Sélectionne une nouvelle position restreinte
                    j++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && j < 100);

                // Si le compteur atteint 50, une erreur est affichée pour indiquer qu'il n'a pas pu trouver une position appropriée
                if (j >= 50)
                {
                    Debug.Log("erreur: ne peut pas créer avec moins de voisins que " + NumberOfNeighbors(checkPos, takenPositions));
                }

                // Une fois une position valide trouvée, la pièce est placée à cette position
                rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);

                // La position est ajoutée à la liste des positions occupées
                takenPositions.Insert(0, checkPos);
            }
        }
    }

    /// <summary>
    /// Génère une nouvelle position adjacente à une des positions déjà prises
    /// La nouvelle position est choisie de manière aléatoire autour d'une position existante
    /// en déplaçant d'une case vers le haut, le bas, la gauche ou la droite.
    /// La position est validée uniquement si elle n'est pas déjà prise et si elle reste dans les limites de la grille
    /// </summary>
    /// <returns>Une nouvelle position valide dans la grille</returns>
    Vector2 NewPosition()
    {
        int x = 0;
        int y = 0;
        Vector2 checkingPos = Vector2.zero; // Stocke temporairement la position à vérifier

        // Boucle qui génère une nouvelle position tant qu'une position valide n'est pas trouvée
        do
        {
            // Prendre une position au hasard parmi les positions déjà occupées.
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x; // Coordonnée X de la position prise au hasard
            y = (int)takenPositions[index].y; // Coordonnée Y de la position prise au hasard

            // Déterminer aléatoirement la direction (haut/bas ou gauche/droite)
            bool upOrDown = (Random.value < 0.5f); // Si vrai, mouvement vertical (haut/bas), sinon mouvement horizontal (gauche/droite)
            bool positive = (Random.value < 0.5f); // Si vrai, mouvement positif (haut/droite), sinon négatif (bas/gauche)

            // Si le mouvement est vertical (haut ou bas)
            if (upOrDown)
            {
                if (positive)
                {
                    y += 1; // Mouvement vers le haut (incrémenter Y)
                }
                else
                {
                    y -= 1; // Mouvement vers le bas (décrémenter Y)
                }
            }
            // Si le mouvement est horizontal (gauche ou droite)
            else
            {
                if (positive)
                {
                    x += 1; // Mouvement vers la droite (incrémenter X)
                }
                else
                {
                    x -= 1; // Mouvement vers la gauche (décrémenter X)
                }
            }

            // La position à vérifier est mise à jour avec les nouvelles coordonnées
            checkingPos = new Vector2(x, y);
        }
        // Répéter la boucle tant que la nouvelle position est déjà prise ou en dehors des limites de la grille
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        // Retourner la nouvelle position valide
        return checkingPos;
    }

    /// <summary>
    /// Génère une nouvelle position adjacente à une des positions déjà prises, mais en sélectionnant
    /// une position de départ qui a au plus un voisin direct. Si aucune position valide n'est trouvée
    /// après 100 tentatives, une erreur est affichée
    /// </summary>
    /// <returns>Une nouvelle position valide dans la grille</returns>
    Vector2 SelectiveNewPosition()
    {
        int index = 0; // Index pour sélectionner une position parmi les positions prises
        int inc = 0;   // Compteur pour limiter les tentatives de recherche
        int x = 0;     // Coordonnée X de la nouvelle position
        int y = 0;     // Coordonnée Y de la nouvelle position
        Vector2 checkingPos = Vector2.zero; // Variable pour stocker la position à vérifier

        // Boucle principale pour générer une position adjacente à une position avec au maximum 1 voisin
        do
        {
            inc = 0; // Réinitialiser le compteur pour chaque nouvelle tentative
            do
            {
                // Sélectionne une position aléatoire dans la liste des positions déjà prises
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++; // Incrémenter le compteur de tentatives
            }
            // Continue à chercher une nouvelle position tant qu'elle a plus d'un voisin et que moins de 100 tentatives ont été faites
            while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

            // Une fois une position appropriée trouvée, on en génère une nouvelle adjacente
            x = (int)takenPositions[index].x; // Coordonnée X de la position sélectionnée
            y = (int)takenPositions[index].y; // Coordonnée Y de la position sélectionnée

            // Détermine aléatoirement si le mouvement est vertical ou horizontal.
            bool upOrDown = (Random.value < 0.5f); // Si vrai, mouvement vertical, sinon horizontal
            bool positive = (Random.value < 0.5f); // Si vrai, mouvement positif (haut/droite), sinon négatif (bas/gauche)

            // Si le mouvement est vertical.
            if (upOrDown)
            {
                if (positive)
                {
                    y += 1; // Mouvement vers le haut (incrémenter Y)
                }
                else
                {
                    y -= 1; // Mouvement vers le bas (décrémenter Y)
                }
            }
            // Si le mouvement est horizontal
            else
            {
                if (positive)
                {
                    x += 1; // Mouvement vers la droite (incrémenter X)
                }
                else
                {
                    x -= 1; // Mouvement vers la gauche (décrémenter X)
                }
            }

            // Mettre à jour la position à vérifier
            checkingPos = new Vector2(x, y);
        }
        // Répète la boucle tant que la nouvelle position est déjà prise ou en dehors des limites de la grille
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        // Si plus de 100 tentatives ont été effectuées, afficher un message d'erreur
        if (inc >= 100)
        {
            Debug.Log("erreur: peut pas trouver la position avec un voisin seulement");
        }

        // Retourner la nouvelle position valide
        return checkingPos;
    }

    /// <summary>
    /// Cette fonction vérifie combien de voisins directs (haut, bas, gauche, droite) la position donnée possède
    /// en comparant la position fournie avec les positions déjà occupées dans la liste usedPositions
    /// </summary>
    /// <param name="checkingPos">La position à vérifier</param>
    /// <param name="usedPositions">Liste des positions déjà occupées</param>
    /// <returns>Le nombre de voisins directs (0 ou 1)</returns>
    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0; // Variable pour stocker le nombre de voisins

        // Vérifie si une position voisine (droite, gauche, haut, bas) est occupée
        if (usedPositions.Contains(checkingPos + Vector2.right) || // Voisin à droite
            usedPositions.Contains(checkingPos + Vector2.left) ||  // Voisin à gauche
            usedPositions.Contains(checkingPos + Vector2.up) ||    // Voisin en haut
            usedPositions.Contains(checkingPos + Vector2.down))    // Voisin en bas
        {
            ret++; // Incrémente si au moins une position voisine est occupée
        }

        // Retourne le nombre total de voisins (0 ou 1, car la fonction ne compte qu'une fois même s'il y a plusieurs voisins)
        return ret;
    }

    /// <summary>
    /// Configure les portes de chaque pièce en fonction des pièces voisines
    /// Chaque pièce peut avoir des portes en haut, en bas, à gauche ou à droite
    /// si une autre pièce existe dans cette direction. Si aucune pièce n'est présente
    /// dans une direction, la porte correspondante est fermée
    /// </summary>
    void SetRoomDoors()
    {
        // Parcourir toutes les pièces dans la grille.
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < (gridSizeY * 2); y++)
            {
                // Si aucune pièce n'est présente à cette position, continuer
                if (rooms[x, y] == null)
                {
                    continue;
                }

                // La position actuelle dans la grille est stockée dans gridPosition
                Vector2 gridPosition = new Vector2(x, y);

                // Vérification de la présence d'une pièce en bas
                if (y - 1 < 0)
                {
                    // Si la position en bas est en dehors de la grille, la porte en bas est fermée
                    rooms[x, y].doorBot = false;
                }
                else
                {
                    // Sinon, vérifier si une pièce existe en bas. Si oui, ouvrir la porte
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }

                // Vérification de la présence d'une pièce en haut
                if (y + 1 >= gridSizeY * 2)
                {
                    // Si la position en haut est en dehors de la grille, la porte en haut est fermée
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    // Sinon, vérifier si une pièce existe en haut. Si oui, ouvrir la porte
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }

                // Vérification de la présence d'une pièce à gauche
                if (x - 1 < 0)
                {
                    // Si la position à gauche est en dehors de la grille, la porte à gauche est fermée
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    // Sinon, vérifier si une pièce existe à gauche. Si oui, ouvrir la porte
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }

                // Vérification de la présence d'une pièce à droite
                if (x + 1 >= gridSizeX * 2)
                {
                    // Si la position à droite est en dehors de la grille, la porte à droite est fermée
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    // Sinon, vérifier si une pièce existe à droite. Si oui, ouvrir la porte
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }
    }

    /// <summary>
    /// Dessine la carte en plaçant les objets de pièce (Room) dans le monde à leurs positions respectives
    /// Les pièces sont positionnées en multipliant leurs coordonnées de grille par des valeurs de facteur d'échelle
    /// </summary>
    void DrawMap()
    {
        // Parcourir toutes les pièces dans la grille
        foreach (Room room in rooms)
        {
            // Si la pièce est vide (null), on passe à la suivante
            if (room == null) continue;

            // Calculer la position de dessin en fonction des coordonnées de la pièce dans la grille
            Vector2 drawPos = room.gridPos;

            // Appliquer une échelle pour espacer correctement les pièces sur la carte (par exemple, 16 unités en X et 8 en Y)
            drawPos.x *= 16;  // Multiplier la position X par 16 (espacement horizontal)
            drawPos.y *= 8;   // Multiplier la position Y par 8 (espacement vertical)

            // Instancier l'objet de la pièce dans le monde
            // Utiliser Instantiate pour créer une instance du GameObject de la pièce (roomWhiteObj) à la position calculée
            Instantiate(roomWhiteObj, drawPos, Quaternion.identity);
        }
    }

}
