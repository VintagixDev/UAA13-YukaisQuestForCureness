using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomManager : MonoBehaviour
{
    [Header("Prefabs")]
    // Prefabs pour la g�n�ration de pi�ces
    [SerializeField] GameObject RoomPrefab;
    [SerializeField] GameObject roomBossPrefabTest;
    [SerializeField] List<GameObject> roomPrefabs;
    [SerializeField] private int maxRoom = 15;
    [SerializeField] private int minRoom = 10;

    // Dimensions pour l'espacement des pi�ces
    int roomWidth = 20;
    int roomHeight = 12;

    [Header("Grid Size")]
    // Dimensions de la grille
    [SerializeField] int gridSizex = 10;
    [SerializeField] int gridSizey = 10;

    // Listes pour stocker les objets des pi�ces g�n�r�es
    private List<GameObject> roomObjects = new List<GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private int[,] roomGrid;
    private int roomCount;
    private bool generationComplete = false;
    private void Start()
    {
        // Initialisation de la grille et d�marrage de la g�n�ration � partir du centre
        roomGrid = new int[gridSizex, gridSizey];
        Vector2Int initialRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
        OpenAllDoors(); // Ouvrir toutes les portes apr�s la g�n�ration des pi�ces
    }
    private void Update()
    {
        // V�rifie si la g�n�ration est compl�te
        if (!generationComplete && roomCount >= minRoom)
        {
            Debug.Log($"G�n�ration compl�te, {roomCount} pi�ces cr��es");
            generationComplete = true;
        }
        else if (roomCount < minRoom)
        {
            Debug.Log($"Pas assez de pi�ces g�n�r�es, recommen�ons.");
            RegenerateRooms(); // Reg�n�rer les pi�ces si le nombre minimum n'est pas atteint
        }
    }
    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        // D�marre la g�n�ration de pi�ces � partir d'un indice donn�
        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        // Instancie la pi�ce initiale
        var initialRoom = Instantiate(RoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"ROOM-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);

        // Continue � g�n�rer des pi�ces jusqu'� atteindre le nombre minimum requis
        while (roomQueue.Count > 0 && roomCount < maxRoom)
        {
            Vector2Int currentRoomIndex = roomQueue.Dequeue();
            GenerateAdjacentRooms(currentRoomIndex);
        }

        // V�rifie apr�s la g�n�ration si nous avons atteint le minimum
        if (roomCount < minRoom)
        {
            Debug.LogWarning("Moins de pi�ces g�n�r�es que pr�vu, essayez � nouveau.");
            RegenerateRooms();
        }
    }
    private void GenerateAdjacentRooms(Vector2Int roomIndex)
    {
        // Essaye de g�n�rer des pi�ces adjacentes
        TryGenerateRoom(new Vector2Int(roomIndex.x - 1, roomIndex.y)); // Gauche
        TryGenerateRoom(new Vector2Int(roomIndex.x + 1, roomIndex.y)); // Droite
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y - 1)); // Bas
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y + 1)); // Haut
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
        roomObjects.Add(newRoom);

        return true;
    }
    private void OpenAllDoors()
    {
        // Ouvre toutes les portes des pi�ces cr��es
        foreach (var roomObject in roomObjects)
        {
            var roomScript = roomObject.GetComponent<Room>();
            int x = roomScript.RoomIndex.x;
            int y = roomScript.RoomIndex.y;
            OpenDoors(roomObject, x, y);
        }
    }
    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        // Ouvrir les portes vers les pi�ces adjacentes
        if (x > 0 && roomGrid[x - 1, y] != 0) // Gauche
        {
            //Debug.Log($"({x}, {y}) vers ({x - 1}, {y})");
            newRoomScript.OpenDoor(Vector2Int.left, roomCount);
            leftRoomScript?.OpenDoor(Vector2Int.right, roomCount);
        }

        if (x < gridSizex - 1 && roomGrid[x + 1, y] != 0) // Droite
        {
            //Debug.Log($"({x}, {y}) vers ({x + 1}, {y})");
            newRoomScript.OpenDoor(Vector2Int.right, roomCount);
            rightRoomScript?.OpenDoor(Vector2Int.left, roomCount);
        }

        if (y > 0 && roomGrid[x, y - 1] != 0) // Bas
        {
            //Debug.Log($"({x}, {y}) vers ({x}, {y - 1})");
            newRoomScript.OpenDoor(Vector2Int.down, roomCount);
            bottomRoomScript?.OpenDoor(Vector2Int.up, roomCount);
        }

        if (y < gridSizey - 1 && roomGrid[x, y + 1] != 0) // Haut
        {
            //Debug.Log($"({x}, {y}) vers ({x}, {y + 1})");
            newRoomScript.OpenDoor(Vector2Int.up, roomCount);
            topRoomScript?.OpenDoor(Vector2Int.down, roomCount);
        }
    }
    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();

        Debug.LogWarning($"Room not found at index: {index}");
        return null;
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
        // Convertit un indice de grille en position pour placer les pi�ces
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizex / 2), roomHeight * (gridY - gridSizey / 2));
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
}
