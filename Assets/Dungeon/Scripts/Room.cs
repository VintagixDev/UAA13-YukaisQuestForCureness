using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour
{
    //[Header("Door Container")]
    //public GameObject AllDoors;

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
    public Vector2Int RoomIndex { get; set; }
    public void OpenDoor(Vector2 direction, int roomCount)
    {
        if (direction == Vector2.up) // Open top door
        {
            Debug.Log("Top door is opening");
            
            if (!topDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(topDoorPrefab, topDoor.transform.position, Quaternion.identity);
                newDoor.name = $"UpDoor";
                Destroy(topDoor);
            }
        }
        if (direction == Vector2.down) // Open bottom door
        {
            Debug.Log("Bottom door is opening");
            
            if (!botDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(botDoorPrefab, botDoor.transform.position, Quaternion.identity);
                newDoor.name = $"BotDoor";
                Destroy(botDoor);
            }
        }
        if (direction == Vector2.left) // Open left door
        {
            Debug.Log("Left door is opening");
            
            if (!leftDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(leftDoorPrefab, leftDoor.transform.position, Quaternion.identity);
                newDoor.name = $"LeftDoor";
                Destroy(leftDoor);
            }
        }
        if (direction == Vector2.right) // Open right door
        {
            Debug.Log("Right door is opening");
            
            if (!rightDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(rightDoorPrefab, rightDoor.transform.position, Quaternion.identity);
                newDoor.name = $"RightDoor";
                Destroy(rightDoor);
            }
        }
    }
}
