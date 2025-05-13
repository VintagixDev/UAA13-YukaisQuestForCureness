using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Room Container")]
    [Tooltip("Stockage des pièces générées")]
    [SerializeField] private Transform AllRooms;

    [Header("Prefabs")]
    [Tooltip("Pièce d'apparition")]
    [SerializeField] private GameObject SpawnRoomPrefabs;
    [Tooltip("Pièce de combat par défaut")]
    [SerializeField] private GameObject RoomPrefab;
    [Tooltip("Pièce de combat de boss par défaut")]
    [SerializeField] private GameObject BossRoomPrefab;
    [Tooltip("Liste de pièce de combat")]
    [SerializeField] private List<GameObject> BattleRoomPrefabs;
    [Tooltip("Liste de pièce non dédié au combat")]
    [SerializeField] private List<GameObject> SpecialRoomPrefabs;

    [Header("Numbers of rooms")] // Peut être modifié plus tard ou dans l'inspecteur
    [Tooltip("Nombre de pièce Maximum")]
    [SerializeField] private int maxRoom = 15;
    [Tooltip("Nombre de pièce Minimum")]
    [SerializeField] private int minRoom = 10;

    [Header("Special Rooms")] // Peut être modifié plus tard ou dans l'inspecteur
    [Tooltip("Nombre de pièce spéciale définie")]
    [SerializeField] private int nbSpRoom = 2;

    [Header("Room max dimensions")] 
    [Tooltip("Taille en longueur")]
    [SerializeField] private int roomWidth = 20;
    [Tooltip("Taille en largeur")]
    [SerializeField] private int roomHeight = 12;

    [Header("Grid Size")] // Peut être modifié plus tard ou dans l'inspecteur
    [Tooltip("Taille de la grille en axe X")]
    [SerializeField] private int gridSizeX = 10;
    [Tooltip("Taille de la grille en axe Y")]
    [SerializeField] private int gridSizeY = 10;

    private List<GameObject> _roomObjects = new List<GameObject>();
    private Queue<Vector2Int> _roomQueue = new Queue<Vector2Int>();
    private int[,] _roomGrid;
    private int _roomCount;

    public void StartDungeonGeneration()
    {
        _roomGrid = new int[gridSizeX, gridSizeY];
        _roomQueue.Clear();
        _roomObjects.Clear();
        _roomCount = 0;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);

        PlaceSpecialRooms();
        OpenAllDoors();
    }
    public void ClearDungeon()
    {
        foreach (GameObject room in _roomObjects)
        {
            Destroy(room);
        }
        _roomObjects.Clear();
        _roomGrid = new int[gridSizeX, gridSizeY];
        _roomQueue.Clear();
        _roomCount = 0;

        Debug.Log("Dungeon cleared.");
    }
    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        _roomQueue.Enqueue(roomIndex);
        _roomGrid[roomIndex.x, roomIndex.y] = 1;

        var initialRoom = Instantiate(SpawnRoomPrefabs, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"SPAWN_ROOM";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        initialRoom.GetComponent<Room>().SetRoomID(_roomCount);
        _roomObjects.Add(initialRoom);
        _roomCount++;

        while (_roomQueue.Count > 0 && _roomCount < maxRoom)
        {
            Vector2Int currentRoomIndex = _roomQueue.Dequeue();
            GenerateAdjacentRooms(currentRoomIndex);
        }

        if (_roomCount >= minRoom)
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

        GameObject extremityRoom = _roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == extremityIndex);
        if (extremityRoom != null)
        {
            _roomObjects.Remove(extremityRoom);
            Destroy(extremityRoom);
        }

        var bossRoom = Instantiate(BossRoomPrefab, GetPositionFromGridIndex(extremityIndex), Quaternion.identity);
        bossRoom.GetComponent<Room>().RoomIndex = extremityIndex;
        bossRoom.name = "BOSS_ROOM";
        _roomObjects.Add(bossRoom);

        _roomGrid[extremityIndex.x, extremityIndex.y] = 2;
    }
    private Vector2Int FindFarthestExtremity()
    {
        Vector2Int startRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        Vector2Int extremityIndex = Vector2Int.zero;
        float maxDistance = 0;

        foreach (var roomObject in _roomObjects)
        {
            Vector2Int roomIndex = roomObject.GetComponent<Room>().RoomIndex;
            float distance = Vector2Int.Distance(startRoomIndex, roomIndex);

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
        TryGenerateRoom(new Vector2Int(roomIndex.x - 1, roomIndex.y));
        TryGenerateRoom(new Vector2Int(roomIndex.x + 1, roomIndex.y));
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y - 1));
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y + 1));
    }
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        if (_roomCount >= maxRoom) return false;
        if (!IsInBounds(roomIndex)) return false;
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;
        if (CountAdjacentRooms(roomIndex) > 1) return false;

        _roomQueue.Enqueue(roomIndex);
        _roomGrid[roomIndex.x, roomIndex.y] = 1;
        _roomCount++;

        GameObject roomToInstantiate = BattleRoomPrefabs[Random.Range(0, BattleRoomPrefabs.Count)];
        GameObject newRoom = Instantiate(roomToInstantiate, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        Room roomScript = newRoom.GetComponent<Room>();
        roomScript.RoomIndex = roomIndex;
        newRoom.name = $"ROOM-{_roomCount}";
        roomScript.SetRoomID(_roomCount);

        _roomObjects.Add(newRoom);
        return true;
    }
    private bool IsInBounds(Vector2Int index)
    {
        return index.x >= 0 && index.x < gridSizeX && index.y >= 0 && index.y < gridSizeY;
    }
    private void OpenAllDoors()
    {
        foreach (var roomObject in _roomObjects)
        {
            var roomScript = roomObject.GetComponent<Room>();
            int x = roomScript.RoomIndex.x;
            int y = roomScript.RoomIndex.y;
            SetDoorsRoomByRoom(roomObject, x, y);
        }
    }
    public void SetDoorsRoomByRoom(GameObject room, int x, int y)
    {
        Room currentRoom = room.GetComponent<Room>();

        if (x > 0 && _roomGrid[x - 1, y] != 0)
        {
            currentRoom.SetDoor(Vector2.left, _roomCount);
            Room leftRoom = GetRoomScriptAt(new Vector2Int(x - 1, y));
            if (leftRoom != null)
            {
                Door currentDoor = currentRoom.GetDoor(Vector2.left);
                Door leftDoor = leftRoom.GetDoor(Vector2.right);
                if (currentDoor != null && leftDoor != null)
                {
                    currentDoor.connectedDoor = leftDoor;
                    leftDoor.connectedDoor = currentDoor;
                }
            }
        }

        if (x < gridSizeX - 1 && _roomGrid[x + 1, y] != 0)
        {
            currentRoom.SetDoor(Vector2.right, _roomCount);
            Room rightRoom = GetRoomScriptAt(new Vector2Int(x + 1, y));
            if (rightRoom != null)
            {
                Door currentDoor = currentRoom.GetDoor(Vector2.right);
                Door rightDoor = rightRoom.GetDoor(Vector2.left);
                if (currentDoor != null && rightDoor != null)
                {
                    currentDoor.connectedDoor = rightDoor;
                    rightDoor.connectedDoor = currentDoor;
                }
            }
        }

        if (y > 0 && _roomGrid[x, y - 1] != 0)
        {
            currentRoom.SetDoor(Vector2.down, _roomCount);
            Room bottomRoom = GetRoomScriptAt(new Vector2Int(x, y - 1));
            if (bottomRoom != null)
            {
                Door currentDoor = currentRoom.GetDoor(Vector2.down);
                Door bottomDoor = bottomRoom.GetDoor(Vector2.up);
                if (currentDoor != null && bottomDoor != null)
                {
                    currentDoor.connectedDoor = bottomDoor;
                    bottomDoor.connectedDoor = currentDoor;
                }
            }
        }

        if (y < gridSizeY - 1 && _roomGrid[x, y + 1] != 0)
        {
            currentRoom.SetDoor(Vector2.up, _roomCount);
            Room topRoom = GetRoomScriptAt(new Vector2Int(x, y + 1));
            if (topRoom != null)
            {
                Door currentDoor = currentRoom.GetDoor(Vector2.up);
                Door topDoor = topRoom.GetDoor(Vector2.down);
                if (currentDoor != null && topDoor != null)
                {
                    currentDoor.connectedDoor = topDoor;
                    topDoor.connectedDoor = currentDoor;
                }
            }
        }
    }
    public Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = _roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();

        Debug.LogWarning($"Room not found at index: {index}");
        return null;
    }
    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && _roomGrid[x - 1, y] != 0) count++;
        if (x < gridSizeX - 1 && _roomGrid[x + 1, y] != 0) count++;
        if (y > 0 && _roomGrid[x, y - 1] != 0) count++;
        if (y < gridSizeY - 1 && _roomGrid[x, y + 1] != 0) count++;

        return count;
    }
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }
    private void RegenerateRooms()
    {
        _roomObjects.ForEach(Destroy);
        _roomObjects.Clear();
        _roomGrid = new int[gridSizeX, gridSizeY];
        _roomQueue.Clear();
        _roomCount = 0;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }
    private void PlaceSpecialRooms()
    {
        int placedSpecialRooms = 0;
        List<Vector2Int> candidates = new List<Vector2Int>();

        foreach (var roomObj in _roomObjects)
        {
            Vector2Int index = roomObj.GetComponent<Room>().RoomIndex;
            if (CountAdjacentRooms(index) == 1 && _roomGrid[index.x, index.y] == 1)
            {
                candidates.Add(index);
            }
        }

        ShuffleList(candidates);

        foreach (var candidate in candidates)
        {
            if (placedSpecialRooms >= nbSpRoom) break;

            GameObject oldRoom = _roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == candidate);
            if (oldRoom != null)
            {
                _roomObjects.Remove(oldRoom);
                Destroy(oldRoom);
            }

            GameObject specialRoomPrefab = SpecialRoomPrefabs[Random.Range(0, SpecialRoomPrefabs.Count)];
            GameObject specialRoom = Instantiate(specialRoomPrefab, GetPositionFromGridIndex(candidate), Quaternion.identity);
            Room roomScript = specialRoom.GetComponent<Room>();
            roomScript.RoomIndex = candidate;
            roomScript.SetRoomID(_roomCount + placedSpecialRooms);
            specialRoom.name = $"SPECIAL_ROOM_{placedSpecialRooms + 1}";

            _roomObjects.Add(specialRoom);
            _roomGrid[candidate.x, candidate.y] = 3;

            placedSpecialRooms++;
        }
    }
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
