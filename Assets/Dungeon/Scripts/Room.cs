using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // pr�fabs pr�_existant servant � garder en m�moire les positions
    [Header("Doors Prefab : Positions")]
    [SerializeField] GameObject topDoor; // HAUT/NORTH
    [SerializeField] GameObject botDoor; // BAS/SOUNTH
    [SerializeField] GameObject leftDoor; // GAUCHE/WEST
    [SerializeField] GameObject rightDoor; // DROITE/EAST

    // pr�fabs des portes pour instanciation
    [Header("Doors Prefab : To Replace")]
    [SerializeField] GameObject DoorPrefab;

    [SerializeField] private int roomID;
    public int RoomID { get { return roomID; } }
    private static int globalDoorIdCount = 1; // ID global des portes
    [SerializeField] public bool isBattleFinished;

    [SerializeField, Tooltip("Si c'est la salle du boss = true")] public bool isBossRoom; 

    [SerializeField] private GameObject[] enemySpawners;
    //public int RoomID; // Identifiant unique de la salle.
    public Vector2Int RoomIndex { get; set; } // Indice de la salle dans une grille (coordonn�es).
    private Dictionary<Vector2, Door> doors = new Dictionary<Vector2, Door>(); // pour stocker les informations des portes

    /// <summary>
    /// D�finit l'identifiant de la salle.
    /// </summary>
    /// <param name="id">Identifiant unique de la salle.</param>
    public void SetRoomID(int id)
    {
        roomID = id;

        if (enemySpawners != null && enemySpawners.Length > 0)
        {
            foreach (var enemy in enemySpawners)
            {
                if (enemy != null)
                {
                    EnemySpawner spawnerScript = enemy.GetComponent<EnemySpawner>();
                    if (spawnerScript != null)
                    {
                        spawnerScript.RoomID = roomID;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Configure une porte dans une direction donn�e et initialise ses propri�t�s.
    /// </summary>
    /// <param name="direction">Direction de la porte (Vector2).</param>
    /// <param name="roomCount">Nombre total de salles dans le niveau.</param>
    /// <param name="isLocked">Indique si la porte est verrouill�e.</param>
    /// <param name="isLockedByBattle">Indique si la porte est verrouill�e par un combat.</param>
    /// <param name="isBossDoor">Indique si la porte est une porte de boss.</param>
    public void SetDoor(Vector2 direction, int roomCount, bool isLocked = false, bool isLockedByBattle = false, bool isBossDoor = false)
    {
        GameObject doorPrefab = null;
        Vector3 position = Vector3.zero;
        string orientation = "";
        string doorName = "";
        //Door connectedDoor = null;
        Vector2Int connectedDoorPosition = Vector2Int.zero;

        // R�cup�ration de la porte et de la position en fonction de la direction
        if (direction == Vector2.up)
        {
            doorPrefab = DoorPrefab;
            position = topDoor.transform.position;
            orientation = "N";
            doorName = "UpDoor-";
            connectedDoorPosition = new Vector2Int(RoomIndex.x, RoomIndex.y + 1); // Coordonn�e de la porte voisine
        }
        else if (direction == Vector2.down)
        {
            doorPrefab = DoorPrefab;
            position = botDoor.transform.position;
            orientation = "S";
            doorName = "BotDoor-";
            connectedDoorPosition = new Vector2Int(RoomIndex.x, RoomIndex.y - 1);
        }
        else if (direction == Vector2.left)
        {
            doorPrefab = DoorPrefab;
            position = leftDoor.transform.position;
            orientation = "W";
            doorName = "LeftDoor-";
            connectedDoorPosition = new Vector2Int(RoomIndex.x - 1, RoomIndex.y);
        }
        else if (direction == Vector2.right)
        {
            doorPrefab = DoorPrefab;
            position = rightDoor.transform.position;
            orientation = "E";
            doorName = "RightDoor-";
            connectedDoorPosition = new Vector2Int(RoomIndex.x + 1, RoomIndex.y);
        }

        if (doorPrefab != null)
        {
            GameObject newDoorObj = Instantiate(doorPrefab, position, Quaternion.identity, transform);
            newDoorObj.name = doorName;

            Door doorScript = newDoorObj.GetComponent<Door>();
            if (doorScript != null)
            {
                int uniqueDoorId = globalDoorIdCount++;

                // Utilisation de doorId pass� en param�tre pour garantir un ID unique
                doorScript.InitializeDoor(uniqueDoorId, roomID, position.x, position.y, orientation, isLocked, isLockedByBattle, isBossDoor);

                // Relier la porte � la porte voisine
                doorScript.connectedDoorPosition = connectedDoorPosition;

                // Ajoutez la porte � un dictionnaire de portes ou � toute autre structure de votre choix
                doors[direction] = doorScript;

                //Debug.Log($"Porte {doorId} instanci�e et reli�e");
            }
            else
            {
                Debug.LogError("Door script not found on instantiated object");
            }
        }
    }

    //public void SetSpawner()
    //{
    //    foreach (var enemySpawner in enemySpawners)
    //    {
    //        enemySpawner._roomID = roomID;
    //    }
    //}

    /// <summary>
    /// R�cup�re une porte dans une direction donn�e.
    /// </summary>
    /// <param name="direction">Direction de la porte (Vector2).</param>
    /// <returns>Instance de la porte correspondante ou null si introuvable.</returns>
    public Door GetDoor(Vector2 direction)
    {
        if (doors.TryGetValue(direction, out Door door))
        {
            return door;
        }
        return null;
    }

    /// <summary>
    /// Connecte deux portes entre deux salles dans des directions oppos�es.
    /// </summary>
    /// <param name="otherRoom">Salle � connecter.</param>
    /// <param name="direction">Direction de la connexion (Vector2).</param>
    public void ConnectDoors(Room otherRoom, Vector2 direction)
    {
        Door thisDoor = GetDoor(direction);
        Door otherDoor = otherRoom.GetDoor(-direction);

        if (thisDoor != null && otherDoor != null)
        {
            thisDoor.connectedDoor = otherDoor;
            otherDoor.connectedDoor = thisDoor;
        }
    }
}