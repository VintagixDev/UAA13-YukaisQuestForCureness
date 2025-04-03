using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomManager : MonoBehaviour
{
    [Header("Room Container")] // Conteneur principal pour toutes les salles générées
    [SerializeField] private Transform AllRooms;

    [Header("Prefabs")]
    [SerializeField] GameObject SpawnRoomPrefabs; // La salle de départ
    [SerializeField] GameObject RoomPrefab; // une salle générique
    [SerializeField] GameObject BossRoomPrefab; // la salle du boss
    [SerializeField] List<GameObject> BattleRoomPrefabs; // Liste des préfabriqués pour les salles de combat
    [SerializeField] List<GameObject> SpecialRoomPrefabs; // Liste des préfabriqués pour les salles spéciales

    [Header("Numbers of rooms")]
    [SerializeField] private int maxRoom = 15; // Nombre maximum de salles à générer
    [SerializeField] private int minRoom = 10; // Nombre minimum de salles à générer

    [Header("Room max dimensions")]
    [SerializeField] int roomWidth = 20; // Largeur générique des salles en unités de jeu
    [SerializeField] int roomHeight = 12; // Hauteur générique des salles en unités de jeu

    [Header("Grid Size")]
    [SerializeField] int gridSizex = 10; // Taille de la grille en X pour la génération des salles
    [SerializeField] int gridSizey = 10; // Taille de la grille en Y pour la génération des salles

    private List<GameObject> roomObjects = new List<GameObject>(); // Liste des objets des salles générées
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>(); // File des indices des salles à générer
    private int[,] roomGrid; // Grille pour suivre les salles générées
    private int roomCount; // Compteur du nombre total de salles générées
    private bool generationComplete = false; // Indique si la génération est terminée
    private int nextRoomID = 1; // ID pour la prochaine salle à générer
    public void StartDungeonGeneration()
    {
        roomGrid = new int[gridSizex, gridSizey];
        roomQueue.Clear();
        roomObjects.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);  

        OpenAllDoors();

        //// Pour le moment on ne peut pas les mettre dans ALLROOMS
        //foreach (var room in roomObjects)
        //{
        //    Debug.Log("Room dans le parent");
        //    room.transform.SetParent(AllRooms, false);
        //}
    }

    public void ClearDungeon()
    {
        foreach (GameObject room in roomObjects)
        {
            Destroy(room);
        }
        roomObjects.Clear();
        roomGrid = new int[gridSizex, gridSizey];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;
        nextRoomID = 1;

        Debug.Log("Dungeon cleared.");
    }

    /// <summary>
    /// Lance la génération des salles à partir d'une salle initiale.
    /// </summary>
    /// <param name="roomIndex"></param>
    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;

        var initialRoom = Instantiate(SpawnRoomPrefabs, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"SPAWN_ROOM"; 
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
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

    /// <summary>
    /// Place une salle de boss à l'extrémité la plus éloignée de la grille.
    /// </summary>
    private void PlaceBossRoomAtExtremity()
    {
        Vector2Int extremityIndex = FindFarthestExtremity();
        if (extremityIndex == Vector2Int.zero) return;

        GameObject extremityRoom = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == extremityIndex);
        if (extremityRoom != null)
        {
            roomObjects.Remove(extremityRoom);
            Destroy(extremityRoom);
        }

        var bossRoom = Instantiate(BossRoomPrefab, GetPositionFromGridIndex(extremityIndex), Quaternion.identity);
        bossRoom.GetComponent<Room>().RoomIndex = extremityIndex;
        bossRoom.name = "BOSS_ROOM";
        roomObjects.Add(bossRoom);

        roomGrid[extremityIndex.x, extremityIndex.y] = 2;
    }

    /// <summary>
    /// Trouve l'indice de la salle la plus éloignée de la salle initiale.
    /// </summary>
    private Vector2Int FindFarthestExtremity()
    {
        Vector2Int startRoomIndex = new Vector2Int(gridSizex / 2, gridSizey / 2);
        Vector2Int extremityIndex = Vector2Int.zero;
        float maxDistance = 0;

        foreach (var roomObject in roomObjects)
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

    /// <summary>
    /// Génère des salles adjacentes à une salle donnée.
    /// </summary>
    /// <param name="roomIndex"></param>
    private void GenerateAdjacentRooms(Vector2Int roomIndex)
    {
        TryGenerateRoom(new Vector2Int(roomIndex.x - 1, roomIndex.y));
        TryGenerateRoom(new Vector2Int(roomIndex.x + 1, roomIndex.y));
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y - 1));
        TryGenerateRoom(new Vector2Int(roomIndex.x, roomIndex.y + 1));
    }

    /// <summary>
    /// Tente de générer une salle à un indice donné.
    /// </summary>
    /// <param name="roomIndex"></param>
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        if (roomCount >= maxRoom) return false;

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;
        if (CountAdjacentRooms(roomIndex) > 1) return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[roomIndex.x, roomIndex.y] = 1;
        Debug.LogWarning($"{roomIndex.x},{roomIndex.y}");
        roomCount++;

        GameObject roomToInstantiate = RoomPrefab;

        GameObject newRoom = Instantiate(roomToInstantiate, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        Room roomScript = newRoom.GetComponent<Room>();
        roomScript.RoomIndex = roomIndex;
        newRoom.name = $"ROOM-{roomCount}";
        roomScript.SetRoomID(roomCount);
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        
        foreach(GameObject enemySpawner in enemySpawners)
        {
            
            EnemySpawner enemySpawnerScript = enemySpawner.GetComponent<EnemySpawner>();
            if (enemySpawnerScript.roomId == -1)
            {
                enemySpawnerScript.roomId = roomCount;
                roomScript.enemySpawners.Add(enemySpawner);
            }
        }
        roomObjects.Add(newRoom);
        return true;
    }

    /// <summary>
    /// Ouvre toutes les portes des salles générées.
    /// </summary>
    private void OpenAllDoors()
    {
        foreach (var roomObject in roomObjects)
        {
            var roomScript = roomObject.GetComponent<Room>();
            int x = roomScript.RoomIndex.x;
            int y = roomScript.RoomIndex.y;
            SetDoorsRoomByRoom(roomObject, x, y);
        }
    }

    /// <summary>
    /// Ouvre les portes d'une salle donnée en fonction de ses pièces adjacentes.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SetDoorsRoomByRoom(GameObject room, int x, int y)
    {
        Room currentRoom = room.GetComponent<Room>();

    // Gauche
    if (x > 0 && roomGrid[x - 1, y] != 0)
    {
        currentRoom.SetDoor(Vector2.left, roomCount);
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

    // Droite
    if (x < gridSizex - 1 && roomGrid[x + 1, y] != 0)
    {
        currentRoom.SetDoor(Vector2.right, roomCount);
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

    // Bas
    if (y > 0 && roomGrid[x, y - 1] != 0)
    {
        currentRoom.SetDoor(Vector2.down, roomCount);
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

    // Haut
    if (y < gridSizey - 1 && roomGrid[x, y + 1] != 0)
    {
        currentRoom.SetDoor(Vector2.up, roomCount);
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

    /// <summary>
    /// Obtient le script de la salle à un indice donné.
    /// </summary>
    /// <param name="index"></param>
    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if (roomObject != null)
            return roomObject.GetComponent<Room>();

        Debug.LogWarning($"Room not found at index: {index}");
        return null;
    }

    /// <summary>
    /// Compte le nombre de salles adjacentes à un indice donné.
    /// </summary>
    /// <param name="roomIndex"></param>
    /// <returns></returns>
    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        // Vérification pour éviter l'accès hors limites
        if (x < 0 || x >= gridSizex || y < 0 || y >= gridSizey)
        {
            //Debug.LogWarning($"Tentative d'accès hors limites : {roomIndex}");
            return 0;
        }

        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++;
        if (x < gridSizex - 1 && roomGrid[x + 1, y] != 0) count++;
        if (y > 0 && roomGrid[x, y - 1] != 0) count++;
        if (y < gridSizey - 1 && roomGrid[x, y + 1] != 0) count++;

        return count;
    }


    /// <summary>
    /// Convertit un indice de grille en une position dans l'espace de jeu.
    /// </summary>
    /// <param name="gridIndex"></param>
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizex / 2), roomHeight * (gridY - gridSizey / 2));
    }

    /// <summary>
    /// Réinitialise les variables et recommence la génération des salles.
    /// </summary>
    private void RegenerateRooms()
    {
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
