using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Prefabs")]
    // Prefabs pour la génération de pièces
    [SerializeField] GameObject RoomPrefab; 
    [SerializeField] GameObject roomBossPrefabTest; 
    [SerializeField] List<GameObject> roomPrefabs; 
    [SerializeField] private int maxRoom = 15; 
    [SerializeField] private int minRoom = 10; 

    // Dimensions pour l'espacement des pièces
    int roomWidth = 20; 
    int roomHeight = 12; 

    [Header("Grid Size")]
    // Dimensions de la grille
    [SerializeField] int gridSizex = 10; 
    [SerializeField] int gridSizey = 10; 

    // Listes pour stocker les objets des pièces générées
    private List<GameObject> roomObjects = new List<GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>(); 

    private int[,] roomGrid; 
    private int roomCount; 
    private bool generationComplete = false; 

    private void Start()
    {
        // Initialisation de la grille et démarrage de la génération à partir du centre
        roomGrid = new int[gridSizex, gridSizey];
        roomQueue = new Queue<Vector2Int>();
        Vector2Int initialRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        // Génération des pièces tant qu'il y a des indices dans la queue et que la limite de pièces n'est pas atteinte
        if (roomQueue.Count > 0 && roomCount < maxRoom && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            // Essaye de générer des pièces adjacentes
            TryGenerateRoom(new Vector2Int(gridX - 1, gridY)); // Gauche
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY)); // Droite
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1)); // Bas
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1)); // Haut
        }
        else if (roomCount < minRoom)
        {
            Debug.Log($"RoomCount était plus petit que le nombre minimal de pièce, recommencer");
            RegenerateRooms(); // Regénère les pièces si le minimum n'est pas atteint
        }
        else if (!generationComplete)
        {
            Debug.Log($"Generation complète, {roomCount} pièces créées");
            generationComplete = true;
        }
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        // Démarre la génération de pièces à partir d'un indice donné
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;

        // Instancie la pièce initiale
        var initialRoom = Instantiate(RoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"ROOM- {roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        // Vérifie si le nombre maximum de pièces est atteint
        if (roomCount >= maxRoom)
        {
            return false;
        }
        // Vérifie si la génération de la pièce doit échouer aléatoirement
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            return false;
        }
        // Vérifie le nombre de pièces adjacentes pour éviter de trop densifier
        if (CountAdjacentRooms(roomIndex) > 1)
        {
            return false;
        }

        // Ajoute la pièce à la queue et à la grille
        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        // Instancie la nouvelle pièce
        var newRoom = Instantiate(RoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"ROOM-{roomCount}";
        OpenDoors(newRoom, x, y);

        return true;
    }

    private void RegenerateRooms()
    {
        // Détruit les pièces existantes et réinitialise les variables pour recommencer la génération
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
        Room newRoomScript = room.GetComponent<Room>();

        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.left,roomCount);
            leftRoomScript.OpenDoor(Vector2Int.right, roomCount);
        }
        if (x < gridSizex - 1 && roomGrid[x + 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.right, roomCount);
            rightRoomScript.OpenDoor(Vector2Int.left, roomCount);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.down, roomCount);
            bottomRoomScript.OpenDoor(Vector2Int.up, roomCount);
        }
        if (y < gridSizey - 1 && roomGrid[x, y + 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.up, roomCount);
            topRoomScript.OpenDoor(Vector2Int.down, roomCount);
        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();
        return null;
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        // Compte le nombre de pièces adjacentes à l'indice donné
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
        // Convertit un indice de grille en position pour placer les pièces
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizex / 2), roomHeight * (gridY - gridSizey / 2));
    }
}
