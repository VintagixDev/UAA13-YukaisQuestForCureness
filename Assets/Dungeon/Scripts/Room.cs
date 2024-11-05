using System.Collections;
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

    [Header("Door Container")]
    [SerializeField] Transform doorsContainer;

    public Vector2Int RoomIndex { get; set; }
    public void OpenDoor(Vector2 direction, int roomCount)
    {
        if (direction == Vector2.up) // Open top door
        {
            Debug.Log("Top door is opening");
            topDoor.SetActive(true);
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
            botDoor.SetActive(true);
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
            leftDoor.SetActive(true);
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
            rightDoor.SetActive(true);
            if (!rightDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(rightDoorPrefab, rightDoor.transform.position, Quaternion.identity);
                newDoor.name = $"RightDooR";
                Destroy(rightDoor);
            }
        }
    }
    public void CloseDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)   
        {
            topDoor.SetActive(false);
        }
        else if (direction == Vector2Int.down) 
        {
            botDoor.SetActive(false);
        }
        else if (direction == Vector2Int.left) 
        {
            leftDoor.SetActive(false);
        }
        else if (direction == Vector2Int.right) 
        {
            rightDoor.SetActive(false);
        }
    }
}
