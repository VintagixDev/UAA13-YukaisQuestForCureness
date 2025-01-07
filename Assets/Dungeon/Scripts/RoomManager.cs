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
    [SerializeField] GameObject SpawnRoomPrefabs;
    [SerializeField] GameObject RoomPrefab;
    [SerializeField] GameObject BossRoomPrefab;
    [SerializeField] List<GameObject> BattleRoomPrefabs;
    [SerializeField] List<GameObject> SpecialRoomPrefabs;

    [Header("Numbers of rooms")]
    [SerializeField] private int maxRoom = 15;
    [SerializeField] private int minRoom = 10;

    [Header("Room max dimensions")]
    [SerializeField] int roomWidth = 20;
    [SerializeField] int roomHeight = 12;

    [Header("Grid Size")]
    [SerializeField] int gridSizex = 10;
    [SerializeField] int gridSizey = 10;

    private List<GameObject> roomObjects = new List<GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private int[,] roomGrid;
    private int roomCount;
    private bool generationComplete = false;

    private int nextRoomID = 1; // ID initial pour la première pièce
    private void Start()
    {
        roomGrid = new int[gridSizex, gridSizey];
        Vector2Int initialRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
        OpenAllDoors();
    }
    private void Update()
    {
        // Vérifie si la génération est complète
        if (!generationComplete && roomCount >= minRoom)
        {
            Debug.Log($"Génération complète, {roomCount} pièces créées");
            generationComplete = true;

            //Mettre les pieces au bonne endroit à gauche
            foreach (var room in roomObjects)
            {
                Debug.Log("Room dans le parent");
                room.transform.SetParent(AllRooms.transform, false);
            }
        }
        else if (roomCount < minRoom)
        {
            Debug.Log($"Pas assez de pièces générées, recommençons.");
            RegenerateRooms(); // Regénérer les pièces si le nombre minimum n'est pas atteint
        }
    }
    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        // Créer la première pièce
        var initialRoom = Instantiate(RoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"ROOM-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;

        // Assigner un ID à la pièce
        initialRoom.GetComponent<Room>().SetRoomID(nextRoomID++);
        roomObjects.Add(initialRoom);

        while (roomQueue.Count > 0 && roomCount < maxRoom)
        {
            Vector2Int currentRoomIndex = roomQueue.Dequeue();
            GenerateAdjacentRooms(currentRoomIndex);
        }

        if (roomCount >= minRoom)
        {
            PlaceBossRoomAtExtremity();
        }
        else
        {
            RegenerateRooms();
        }
    }
    private void PlaceBossRoomAtExtremity()
    {
        Vector2Int extremityIndex = FindFarthestExtremity();
        if (extremityIndex == Vector2Int.zero) return;

        // Détruire l'ancienne pièce de l'extrémité pour la remplacer par la BossRoom
        GameObject extremityRoom = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == extremityIndex);
        if (extremityRoom != null)
        {
            roomObjects.Remove(extremityRoom);
            Destroy(extremityRoom);
        }

        // Instancie la BossRoom à l'extrémité
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

            // Vérifie que la pièce est une extrémité (une seule pièce adjacente)
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
        // Essaye de générer des pièces adjacentes
        TryGenerateRoom(new Vector2Int(roomIndex.x - 1, roomIndex.y)); // Gauche
        TryGenerateRoom(new Vector2Int(roomIndex.x + 1, roomIndex.y)); // Droite
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y - 1)); // Bas
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y + 1)); // Haut
    }
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        if (roomCount >= maxRoom) return false;

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;
        if (CountAdjacentRooms(roomIndex) > 1) return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        roomCount++;

        GameObject roomToInstantiate = RoomPrefab; 

        GameObject newRoom = Instantiate(roomToInstantiate, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"ROOM-{roomCount}";

        // Assigner un ID à la pièce
        newRoom.GetComponent<Room>().SetRoomID(nextRoomID++);
        roomObjects.Add(newRoom);
        return true;
    }
    private void OpenAllDoors()
    {
        // Ouvre toutes les portes des pièces créées
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

        // Ouvrir les portes vers les pièces adjacentes
        if (x > 0 && roomGrid[x - 1, y] != 0) // Gauche
        {
            newRoomScript.SetDoor(Vector2Int.left, roomCount);
        }
        if (x < gridSizex - 1 && roomGrid[x + 1, y] != 0) // Droite
        {
            newRoomScript.SetDoor(Vector2Int.right, roomCount);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0) // Bas
        {
            newRoomScript.SetDoor(Vector2Int.down, roomCount);
        }
        if (y < gridSizey - 1 && roomGrid[x, y + 1] != 0) // Haut
        {
            newRoomScript.SetDoor(Vector2Int.up, roomCount);
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
}