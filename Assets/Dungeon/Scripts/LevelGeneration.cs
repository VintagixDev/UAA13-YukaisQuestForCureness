using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    /// <summary>
    /// Contient les informations sur la taille du monde
    /// worldSize d�finit la taille de la grille dans laquelle les pi�ces seront plac�es
    /// takenPositions garde une trace des positions d�j� occup�es dans cette grille
    /// </summary>
    [Header("World data")]
    Vector2 worldSize = new Vector2(4, 4);
    List<Vector2> takenPositions = new List<Vector2>();

    /// <summary>
    /// Contient les informations relatives aux pi�ces
    /// rooms est un tableau bidimensionnel repr�sentant toutes les pi�ces dans la grille
    /// numberOfRooms est le nombre total de pi�ces � g�n�rer
    /// roomWhiteObj est le prefab (objet) utilis� pour repr�senter les pi�ces dans la sc�ne
    /// </summary>
    [Header("Rooms data")]
    Room[,] rooms;
    int numberOfRooms = 20;
    public GameObject roomWhiteObj;

    /// <summary>
    /// Stocke les dimensions de la grille utilis�e pour g�n�rer le niveau
    /// gridSizeX et gridSizeY repr�sentent respectivement la largeur et la hauteur de la grille
    /// </summary>
    [Header("Grid")]
    int gridSizeX;
    int gridSizeY;

    void Start()
    {
        // On s'assure qu'il n'y a pas plus de pi�ce que de place dans la grille
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
    /// Cette fonction cr�e les pi�ces dans une grille de taille gridSizeX * 2 par gridSizeY * 2
    /// Elle positionne chaque pi�ce al�atoirement en �vitant de placer des pi�ces avec plus d'un voisin imm�diat,
    /// et r�gule la probabilit� de placement en fonction de la progression du nombre de pi�ces cr��es
    /// </summary>
    void CreateRooms()
    {
        // Initialisation de la grille avec la taille d�finie, multipli�e par deux pour chaque dimension
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];

        // Placement de la premi�re pi�ce (position centrale dans la grille) avec un type de pi�ce de valeur 1
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);

        // La position de d�part est (0, 0), donc elle est ajout�e � la liste des positions occup�es
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero; // Variable temporaire pour stocker les nouvelles positions

        // Variables pour r�guler la probabilit� de placement d'une pi�ce avec plusieurs voisins
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        // Boucle pour ajouter le nombre d�fini de pi�ces (20 pi�ces par d�faut, moins 1 car la premi�re pi�ce est d�j� plac�e)
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            // Calcul du pourcentage de progression du placement des pi�ces
            float randomPerc = ((float)i) / ((float)numberOfRooms - 1);

            // Interpolation lin�aire pour ajuster la probabilit� de placement d'une pi�ce avec plusieurs voisins
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            // Obtenir une nouvelle position al�atoire pour une pi�ce
            checkPos = NewPosition();

            // Si la nouvelle position a plus d'un voisin ou si la probabilit� al�atoire est sup�rieure � randomCompare :
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int j = 0; // Compteur pour limiter les tentatives

                // Cherche une nouvelle position tant qu'elle a trop de voisins et que le nombre de tentatives est inf�rieur � 100
                do
                {
                    checkPos = SelectiveNewPosition(); // S�lectionne une nouvelle position restreinte
                    j++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && j < 100);

                // Si le compteur atteint 50, une erreur est affich�e pour indiquer qu'il n'a pas pu trouver une position appropri�e
                if (j >= 50)
                {
                    Debug.Log("erreur: ne peut pas cr�er avec moins de voisins que " + NumberOfNeighbors(checkPos, takenPositions));
                }

                // Une fois une position valide trouv�e, la pi�ce est plac�e � cette position
                rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);

                // La position est ajout�e � la liste des positions occup�es
                takenPositions.Insert(0, checkPos);
            }
        }
    }

    /// <summary>
    /// G�n�re une nouvelle position adjacente � une des positions d�j� prises
    /// La nouvelle position est choisie de mani�re al�atoire autour d'une position existante
    /// en d�pla�ant d'une case vers le haut, le bas, la gauche ou la droite.
    /// La position est valid�e uniquement si elle n'est pas d�j� prise et si elle reste dans les limites de la grille
    /// </summary>
    /// <returns>Une nouvelle position valide dans la grille</returns>
    Vector2 NewPosition()
    {
        int x = 0;
        int y = 0;
        Vector2 checkingPos = Vector2.zero; // Stocke temporairement la position � v�rifier

        // Boucle qui g�n�re une nouvelle position tant qu'une position valide n'est pas trouv�e
        do
        {
            // Prendre une position au hasard parmi les positions d�j� occup�es.
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x; // Coordonn�e X de la position prise au hasard
            y = (int)takenPositions[index].y; // Coordonn�e Y de la position prise au hasard

            // D�terminer al�atoirement la direction (haut/bas ou gauche/droite)
            bool upOrDown = (Random.value < 0.5f); // Si vrai, mouvement vertical (haut/bas), sinon mouvement horizontal (gauche/droite)
            bool positive = (Random.value < 0.5f); // Si vrai, mouvement positif (haut/droite), sinon n�gatif (bas/gauche)

            // Si le mouvement est vertical (haut ou bas)
            if (upOrDown)
            {
                if (positive)
                {
                    y += 1; // Mouvement vers le haut (incr�menter Y)
                }
                else
                {
                    y -= 1; // Mouvement vers le bas (d�cr�menter Y)
                }
            }
            // Si le mouvement est horizontal (gauche ou droite)
            else
            {
                if (positive)
                {
                    x += 1; // Mouvement vers la droite (incr�menter X)
                }
                else
                {
                    x -= 1; // Mouvement vers la gauche (d�cr�menter X)
                }
            }

            // La position � v�rifier est mise � jour avec les nouvelles coordonn�es
            checkingPos = new Vector2(x, y);
        }
        // R�p�ter la boucle tant que la nouvelle position est d�j� prise ou en dehors des limites de la grille
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        // Retourner la nouvelle position valide
        return checkingPos;
    }

    /// <summary>
    /// G�n�re une nouvelle position adjacente � une des positions d�j� prises, mais en s�lectionnant
    /// une position de d�part qui a au plus un voisin direct. Si aucune position valide n'est trouv�e
    /// apr�s 100 tentatives, une erreur est affich�e
    /// </summary>
    /// <returns>Une nouvelle position valide dans la grille</returns>
    Vector2 SelectiveNewPosition()
    {
        int index = 0; // Index pour s�lectionner une position parmi les positions prises
        int inc = 0;   // Compteur pour limiter les tentatives de recherche
        int x = 0;     // Coordonn�e X de la nouvelle position
        int y = 0;     // Coordonn�e Y de la nouvelle position
        Vector2 checkingPos = Vector2.zero; // Variable pour stocker la position � v�rifier

        // Boucle principale pour g�n�rer une position adjacente � une position avec au maximum 1 voisin
        do
        {
            inc = 0; // R�initialiser le compteur pour chaque nouvelle tentative
            do
            {
                // S�lectionne une position al�atoire dans la liste des positions d�j� prises
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++; // Incr�menter le compteur de tentatives
            }
            // Continue � chercher une nouvelle position tant qu'elle a plus d'un voisin et que moins de 100 tentatives ont �t� faites
            while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

            // Une fois une position appropri�e trouv�e, on en g�n�re une nouvelle adjacente
            x = (int)takenPositions[index].x; // Coordonn�e X de la position s�lectionn�e
            y = (int)takenPositions[index].y; // Coordonn�e Y de la position s�lectionn�e

            // D�termine al�atoirement si le mouvement est vertical ou horizontal.
            bool upOrDown = (Random.value < 0.5f); // Si vrai, mouvement vertical, sinon horizontal
            bool positive = (Random.value < 0.5f); // Si vrai, mouvement positif (haut/droite), sinon n�gatif (bas/gauche)

            // Si le mouvement est vertical.
            if (upOrDown)
            {
                if (positive)
                {
                    y += 1; // Mouvement vers le haut (incr�menter Y)
                }
                else
                {
                    y -= 1; // Mouvement vers le bas (d�cr�menter Y)
                }
            }
            // Si le mouvement est horizontal
            else
            {
                if (positive)
                {
                    x += 1; // Mouvement vers la droite (incr�menter X)
                }
                else
                {
                    x -= 1; // Mouvement vers la gauche (d�cr�menter X)
                }
            }

            // Mettre � jour la position � v�rifier
            checkingPos = new Vector2(x, y);
        }
        // R�p�te la boucle tant que la nouvelle position est d�j� prise ou en dehors des limites de la grille
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        // Si plus de 100 tentatives ont �t� effectu�es, afficher un message d'erreur
        if (inc >= 100)
        {
            Debug.Log("erreur: peut pas trouver la position avec un voisin seulement");
        }

        // Retourner la nouvelle position valide
        return checkingPos;
    }

    /// <summary>
    /// Cette fonction v�rifie combien de voisins directs (haut, bas, gauche, droite) la position donn�e poss�de
    /// en comparant la position fournie avec les positions d�j� occup�es dans la liste usedPositions
    /// </summary>
    /// <param name="checkingPos">La position � v�rifier</param>
    /// <param name="usedPositions">Liste des positions d�j� occup�es</param>
    /// <returns>Le nombre de voisins directs (0 ou 1)</returns>
    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0; // Variable pour stocker le nombre de voisins

        // V�rifie si une position voisine (droite, gauche, haut, bas) est occup�e
        if (usedPositions.Contains(checkingPos + Vector2.right) || // Voisin � droite
            usedPositions.Contains(checkingPos + Vector2.left) ||  // Voisin � gauche
            usedPositions.Contains(checkingPos + Vector2.up) ||    // Voisin en haut
            usedPositions.Contains(checkingPos + Vector2.down))    // Voisin en bas
        {
            ret++; // Incr�mente si au moins une position voisine est occup�e
        }

        // Retourne le nombre total de voisins (0 ou 1, car la fonction ne compte qu'une fois m�me s'il y a plusieurs voisins)
        return ret;
    }

    /// <summary>
    /// Configure les portes de chaque pi�ce en fonction des pi�ces voisines
    /// Chaque pi�ce peut avoir des portes en haut, en bas, � gauche ou � droite
    /// si une autre pi�ce existe dans cette direction. Si aucune pi�ce n'est pr�sente
    /// dans une direction, la porte correspondante est ferm�e
    /// </summary>
    void SetRoomDoors()
    {
        // Parcourir toutes les pi�ces dans la grille.
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < (gridSizeY * 2); y++)
            {
                // Si aucune pi�ce n'est pr�sente � cette position, continuer
                if (rooms[x, y] == null)
                {
                    continue;
                }

                // La position actuelle dans la grille est stock�e dans gridPosition
                Vector2 gridPosition = new Vector2(x, y);

                // V�rification de la pr�sence d'une pi�ce en bas
                if (y - 1 < 0)
                {
                    // Si la position en bas est en dehors de la grille, la porte en bas est ferm�e
                    rooms[x, y].doorBot = false;
                }
                else
                {
                    // Sinon, v�rifier si une pi�ce existe en bas. Si oui, ouvrir la porte
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }

                // V�rification de la pr�sence d'une pi�ce en haut
                if (y + 1 >= gridSizeY * 2)
                {
                    // Si la position en haut est en dehors de la grille, la porte en haut est ferm�e
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    // Sinon, v�rifier si une pi�ce existe en haut. Si oui, ouvrir la porte
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }

                // V�rification de la pr�sence d'une pi�ce � gauche
                if (x - 1 < 0)
                {
                    // Si la position � gauche est en dehors de la grille, la porte � gauche est ferm�e
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    // Sinon, v�rifier si une pi�ce existe � gauche. Si oui, ouvrir la porte
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }

                // V�rification de la pr�sence d'une pi�ce � droite
                if (x + 1 >= gridSizeX * 2)
                {
                    // Si la position � droite est en dehors de la grille, la porte � droite est ferm�e
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    // Sinon, v�rifier si une pi�ce existe � droite. Si oui, ouvrir la porte
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }
    }

    /// <summary>
    /// Dessine la carte en pla�ant les objets de pi�ce (Room) dans le monde � leurs positions respectives
    /// Les pi�ces sont positionn�es en multipliant leurs coordonn�es de grille par des valeurs de facteur d'�chelle
    /// </summary>
    void DrawMap()
    {
        // Parcourir toutes les pi�ces dans la grille
        foreach (Room room in rooms)
        {
            // Si la pi�ce est vide (null), on passe � la suivante
            if (room == null) continue;

            // Calculer la position de dessin en fonction des coordonn�es de la pi�ce dans la grille
            Vector2 drawPos = room.gridPos;

            // Appliquer une �chelle pour espacer correctement les pi�ces sur la carte (par exemple, 16 unit�s en X et 8 en Y)
            drawPos.x *= 16;  // Multiplier la position X par 16 (espacement horizontal)
            drawPos.y *= 8;   // Multiplier la position Y par 8 (espacement vertical)

            // Instancier l'objet de la pi�ce dans le monde
            // Utiliser Instantiate pour cr�er une instance du GameObject de la pi�ce (roomWhiteObj) � la position calcul�e
            Instantiate(roomWhiteObj, drawPos, Quaternion.identity);
        }
    }

}
