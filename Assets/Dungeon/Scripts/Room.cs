using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Doors Prefab : Positions")]
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject botDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [Header("Doors Prefab : To Replace")]
    [SerializeField] GameObject topDoorPrefab;
    [SerializeField] GameObject botDoorPrefab;
    [SerializeField] GameObject leftDoorPrefab;
    [SerializeField] GameObject rightDoorPrefab;

    [SerializeField] private int roomID;
    public int RoomID => roomID;
    public Vector2Int RoomIndex { get; set; }

    // pour stocker les informations des portes
    private Dictionary<Vector2, Door> doors = new Dictionary<Vector2, Door>();
    public void SetRoomID(int id)
    {
        roomID = id;
    }
    public void SetDoor(Vector2 direction, int roomCount, bool isLocked = false, bool isLockedByBattle = false, bool isBossDoor = false)
    {
        GameObject doorPrefab = null;
        Vector3 position = Vector3.zero;
        string orientation = "";
        string doorName = "";

        if (direction == Vector2.up)
        {
            doorPrefab = topDoorPrefab;
            position = topDoor.transform.position;
            orientation = "N";
            doorName = "UpDoor";
            Destroy(topDoor);
        }
        else if (direction == Vector2.down)
        {
            doorPrefab = botDoorPrefab;
            position = botDoor.transform.position;
            orientation = "S";
            doorName = "BotDoor";
            Destroy(botDoor);
        }
        else if (direction == Vector2.left)
        {
            doorPrefab = leftDoorPrefab;
            position = leftDoor.transform.position;
            orientation = "W";
            doorName = "LeftDoor";
            Destroy(leftDoor);
        }
        else if (direction == Vector2.right)
        {
            doorPrefab = rightDoorPrefab;
            position = rightDoor.transform.position;
            orientation = "E";
            doorName = "RightDoor";
            Destroy(rightDoor);
        }

        if (doorPrefab != null)
        {
            GameObject newDoorObj = Instantiate(doorPrefab, position, Quaternion.identity, transform);
            newDoorObj.name = doorName;

            Door doorScript = newDoorObj.GetComponent<Door>();
            if (doorScript != null)
            {
                string doorId = $"Door_{roomID}_{doorName}";
                doorScript.InitializeDoor(doorId, position.x, position.y, orientation, isLocked, isLockedByBattle, isBossDoor);
                doors[direction] = doorScript;
                Debug.Log($"Door {doorId} instantiated and initialized");
            }
            else
            {
                Debug.LogError("Door script not found on instantiated object");
            }
        }
    }
    public Door GetDoor(Vector2 direction)
    {
        if (doors.TryGetValue(direction, out Door door))
        {
            return door;
        }
        return null;
    }
}

// Ancienne fonction

//public void OpenDoor(Vector2 direction, int roomCount)
//{
//    if (direction == Vector2.up) // Open top door
//    {
//        Debug.Log("Top door is opening");
//        if (!topDoor.activeInHierarchy)
//        {
//            var newDoor = Instantiate(topDoorPrefab, topDoor.transform.position, Quaternion.identity);
//            newDoor.name = $"UpDoor";
//            Destroy(topDoor);
//        }
//    }
//    if (direction == Vector2.down) // Open bottom door
//    {
//        Debug.Log("Bottom door is opening");
//        if (!botDoor.activeInHierarchy)
//        {
//            var newDoor = Instantiate(botDoorPrefab, botDoor.transform.position, Quaternion.identity);
//            newDoor.name = $"BotDoor";
//            Destroy(botDoor);
//        }
//    }
//    if (direction == Vector2.left) // Open left door
//    {
//        Debug.Log("Left door is opening");
//        if (!leftDoor.activeInHierarchy)
//        {
//            var newDoor = Instantiate(leftDoorPrefab, leftDoor.transform.position, Quaternion.identity);
//            newDoor.name = $"LeftDoor";
//            Destroy(leftDoor);
//        }
//    }
//    if (direction == Vector2.right) // Open right door
//    {
//        Debug.Log("Right door is opening");
//        if (!rightDoor.activeInHierarchy)
//        {
//            var newDoor = Instantiate(rightDoorPrefab, rightDoor.transform.position, Quaternion.identity);
//            newDoor.name = $"RightDoor";
//            Destroy(rightDoor);
//        }
//    }
//}