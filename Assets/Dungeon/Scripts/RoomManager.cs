using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomManager : MonoBehaviour
{
    [Header("Room Container")]
    public GameObject AllRooms;

    [Header("Door Container")]
    public Transform doorContainer;

    [Header("Prefabs")]
    // Prefabs pour la g�n�ration de pi�ces
    [SerializeField] GameObject SpawnRoomPrefabs; // R�serv� pour le spawn
    [SerializeField] GameObject RoomPrefab; // Piece basique et ne doit jamais etre vide
    [SerializeField] GameObject BossRoomPrefab;
    [SerializeField] List<GameObject> BattleRoomPrefabs;
    [SerializeField] List<GameObject> SpecialRoomPrefabs;

    [Header("Numbers of rooms")]
    // Nombre de pieces max et min
    [SerializeField] private int maxRoom = 15;
    [SerializeField] private int minRoom = 10;

    [Header("Prefabs")]
    // Pourcentage de type de pieces
    [SerializeField] int BattleRoomPurcent;
    [SerializeField] int SpecialRoomPurcent;

    [Header("Room max dimensions")]
    // Dimensions pour l'espacement des pi�ces
    [SerializeField] int roomWidth = 20;
    [SerializeField] int roomHeight = 12;

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
        roomGrid = new int[gridSizex, gridSizey];
        Vector2Int initialRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
        OpenAllDoors();
    }
    private void Update()
    {
        // V�rifie si la g�n�ration est compl�te
        if (!generationComplete && roomCount >= minRoom)
        {
            Debug.Log($"G�n�ration compl�te, {roomCount} pi�ces cr��es");
            generationComplete = true;

            //Mettre les pieces au bonne endroit � gauche
            foreach (var room in roomObjects)
            {
                Debug.Log("Room dans le parent");
                room.transform.SetParent(AllRooms.transform, false);
            }
        }
        else if (roomCount < minRoom)
        {
            Debug.Log($"Pas assez de pi�ces g�n�r�es, recommen�ons.");
            RegenerateRooms(); // Reg�n�rer les pi�ces si le nombre minimum n'est pas atteint
        }
    }
    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        // Initialisation de la file d'attente et de la premi�re pi�ce
        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        var initialRoom = Instantiate(RoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"ROOM-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);

        // G�n�rer les pi�ces adjacentes jusqu'� atteindre le maximum ou minimum requis
        while (roomQueue.Count > 0 && roomCount < maxRoom)
        {
            Vector2Int currentRoomIndex = roomQueue.Dequeue();
            GenerateAdjacentRooms(currentRoomIndex);
        }

        // Placer la salle de boss � l'extr�mit� la plus �loign�e si le minimum de pi�ces est atteint
        if (roomCount >= minRoom)
        {
            PlaceBossRoomAtExtremity(); // Cr�e la BossRoom � l'extr�mit�
        }
        else
        {
            Debug.LogWarning("Moins de pi�ces g�n�r�es que pr�vu, essayez � nouveau.");
            RegenerateRooms();
        }
    }
    private void PlaceBossRoomAtExtremity()
    {
        Vector2Int extremityIndex = FindFarthestExtremity();
        if (extremityIndex == Vector2Int.zero) return;

        // D�truire l'ancienne pi�ce de l'extr�mit� pour la remplacer par la BossRoom
        GameObject extremityRoom = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == extremityIndex);
        if (extremityRoom != null)
        {
            roomObjects.Remove(extremityRoom);
            Destroy(extremityRoom);
        }

        // Instancie la BossRoom � l'extr�mit�
        var bossRoom = Instantiate(BossRoomPrefab, GetPositionFromGridIndex(extremityIndex), Quaternion.identity);
        bossRoom.GetComponent<Room>().RoomIndex = extremityIndex;
        bossRoom.name = "BOSS_ROOM";
        roomObjects.Add(bossRoom);

        // Marque la BossRoom dans la grille
        roomGrid[extremityIndex.x, extremityIndex.y] = 2; // 2 pour indiquer la BossRoom
    }
    private Vector2Int FindFarthestExtremity()
    {
        Vector2Int startRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        Vector2Int extremityIndex = Vector2Int.zero;
        float maxDistance = 0;

        foreach (var roomObject in roomObjects)
        {
            Vector2Int roomIndex = roomObject.GetComponent<Room>().RoomIndex;
            float distance = Vector2Int.Distance(startRoomIndex, roomIndex);

            // V�rifie que la pi�ce est une extr�mit� (une seule pi�ce adjacente)
            if (distance > maxDistance && CountAdjacentRooms(roomIndex) == 1)
            {
                maxDistance = distance;
                extremityIndex = roomIndex;
            }
        }

        return extremityIndex;
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
        if (roomCount >= maxRoom) return false;

        // V�rification des conditions pour �viter la surpopulation des pi�ces
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;
        if (CountAdjacentRooms(roomIndex) > 1) return false;

        // Enfile la pi�ce dans la file d'attente et enregistre sa position dans la grille
        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        // D�terminer le type de pr�fab � instancier en fonction des pourcentages
        GameObject roomToInstantiate;
        float roomTypeRoll = Random.Range(0f, 100f);

        if (roomTypeRoll < BattleRoomPurcent && BattleRoomPrefabs.Count > 0)
        {
            // Choisir al�atoirement un pr�fab de BattleRoomPrefabs
            roomToInstantiate = BattleRoomPrefabs[Random.Range(0, BattleRoomPrefabs.Count)];
        }
        else if (roomTypeRoll < BattleRoomPurcent + SpecialRoomPurcent && SpecialRoomPrefabs.Count > 0)
        {
            // Choisir al�atoirement un pr�fab de SpecialRoomPrefabs
            roomToInstantiate = SpecialRoomPrefabs[Random.Range(0, SpecialRoomPrefabs.Count)];
        }
        else
        {
            // Si aucun crit�re n'est rempli, utiliser le pr�fab de base
            roomToInstantiate = RoomPrefab;
        }

        // Instancier la pi�ce choisie et l'ajouter � la liste des pi�ces

        GameObject newRoom = Instantiate(roomToInstantiate, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
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