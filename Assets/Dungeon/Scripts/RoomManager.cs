using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Prefabs pour la g�n�ration de pi�ces
    [SerializeField] GameObject RoomPrefab; // Le prefab de la pi�ce standard
    [SerializeField] GameObject roomBossPrefabTest; // Un prefab pour une pi�ce de boss (pour l'instant utilis� comme test)
    [SerializeField] List<GameObject> roomPrefabs; // Liste des prefabs de pi�ces pour la vari�t�
    [SerializeField] private int maxRoom = 15; // Nombre maximum de pi�ces � g�n�rer
    [SerializeField] private int minRoom = 10; // Nombre minimum de pi�ces � g�n�rer

    // Dimensions pour l'espacement des pi�ces
    int roomWidth = 20; // Largeur d'une pi�ce
    int roomHeight = 12; // Hauteur d'une pi�ce

    // Dimensions de la grille
    [SerializeField] int gridSizex = 10; // Taille de la grille sur l'axe X
    [SerializeField] int gridSizey = 10; // Taille de la grille sur l'axe Y

    // Listes pour stocker les objets des pi�ces g�n�r�es
    private List<GameObject> roomObjects = new List<GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>(); // Queue pour g�rer les indices des pi�ces � g�n�rer

    private int[,] roomGrid; // Grille 2D pour suivre les emplacements des pi�ces
    private int roomCount; // Compteur de pi�ces g�n�r�es
    private bool generationComplete = false; // Indicateur si la g�n�ration est termin�e

    private void Start()
    {
        // Initialisation de la grille et d�marrage de la g�n�ration � partir du centre
        roomGrid = new int[gridSizex, gridSizey];
        roomQueue = new Queue<Vector2Int>();
        Vector2Int initialRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        StartRoomGenerationFromRoom(initialRoomIndex); 
    }

    private void Update()
    {
        // G�n�ration des pi�ces tant qu'il y a des indices dans la queue et que la limite de pi�ces n'est pas atteinte
        if (roomQueue.Count > 0 && roomCount < maxRoom && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue(); 
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            // Essaye de g�n�rer des pi�ces adjacentes
            TryGenerateRoom(new Vector2Int(gridX - 1, gridY)); // Gauche
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY)); // Droite
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1)); // Bas
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1)); // Haut
        }
        else if (roomCount < minRoom)
        {
            Debug.Log($"RoomCount �tait plus petit que le nombre minimal de pi�ce, recommencer");
            RegenerateRooms(); // Reg�n�re les pi�ces si le minimum n'est pas atteint
        }
        else if (!generationComplete)
        {
            Debug.Log($"Generation compl�te, {roomCount} pi�ces cr��es");
            generationComplete = true; 
        }
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        // D�marre la g�n�ration de pi�ces � partir d'un indice donn�
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1; 
        roomCount++; 

        // Instancie la pi�ce initiale
        var initialRoom = Instantiate(RoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"ROOM- {roomCount}"; 
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom); 
    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        // V�rifie si le nombre maximum de pi�ces est atteint
        if (roomCount >= maxRoom)
        {
            return false;
        }
        // V�rifie si la g�n�ration de la pi�ce doit �chouer al�atoirement
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            return false;
        }
        // V�rifie le nombre de pi�ces adjacentes pour �viter de trop densifier
        if (CountAdjacentRooms(roomIndex) > 1)
        {
            return false;
        }

        // Ajoute la pi�ce � la queue et � la grille
        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++; 

        // Instancie la nouvelle pi�ce
        var newRoom = Instantiate(RoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex; 
        newRoom.name = $"ROOM-{roomCount}"; 
        OpenDoors(newRoom, x, y); 

        return true; 
    }

    private void RegenerateRooms()
    {
        // D�truit les pi�ces existantes et r�initialise les variables pour recommencer la g�n�ration
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizex, gridSizey];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        StartRoomGenerationFromRoom(initialRoomIndex); 
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        // G�re l'ouverture des portes entre la pi�ce actuelle et les pi�ces adjacentes
        Room newRoomScript = room.GetComponent<Room>();

        // R�cup�re les scripts des pi�ces adjacentes
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y)); // Gauche
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y)); // Droite
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1)); // Haut
        Room botRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1)); // Bas

        // Ouvre les portes selon les pi�ces adjacentes existantes
        if (x > 0 && roomGrid[x - 1, y] != 0 && leftRoomScript != null) // Gauche
        {
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript.OpenDoor(Vector2Int.right);
        }
        if (x < gridSizex - 1 && roomGrid[x + 1, y] != 0 && rightRoomScript != null) // Droite
        {
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript.OpenDoor(Vector2Int.left);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0 && botRoomScript != null) // Bas
        {
            newRoomScript.OpenDoor(Vector2Int.down);
            botRoomScript.OpenDoor(Vector2Int.up);
        }
        if (y < gridSizey - 1 && roomGrid[x, y + 1] != 0 && topRoomScript != null) // Haut
        {
            newRoomScript.OpenDoor(Vector2Int.up);
            topRoomScript.OpenDoor(Vector2Int.down);
        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        // R�cup�re le script Room d'une pi�ce � l'indice donn�
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
        {
            return roomObject.GetComponent<Room>();
        }
        else
        {
            return null; 
        }
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        // Compte le nombre de pi�ces adjacentes � l'indice donn�
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) // Gauche
        {
            count++;
        }
        if (x < gridSizex - 1 && roomGrid[x + 1, y] != 0) // Droite
        {
            count++;
        }
        if (y > 0 && roomGrid[x, y - 1] != 0) // Bas
        {
            count++;
        }
        if (y < gridSizey - 1 && roomGrid[x, y + 1] != 0) // Haut
        {
            count++;
        }

        return count; 
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        // Convertit un indice de grille en position mondiale pour placer les pi�ces
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizex / 2), roomHeight * (gridY - gridSizey / 2));
    }

    private void OnDrawGizmos()
    {
        // Dessine les contours de la grille dans l'�diteur pour le d�bogage
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizex; x++)
        {
            for (int y = 0; y < gridSizey; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
}
