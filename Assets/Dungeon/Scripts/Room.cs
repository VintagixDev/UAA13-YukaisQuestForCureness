using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Doors Prefab")]
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject botDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [Header("Door Container")]
    [SerializeField] Transform doorsContainer; // conteneur des portes

    public Vector2Int RoomIndex { get; set; }

    public void OpenDoor(Vector2 direction, int roomCount)
    {
        if (direction == Vector2.up)      // Open top door
        {
            Debug.Log("Top door is opening");
            topDoor.SetActive(true);
            if (!topDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(topDoor, topDoor.transform.position, Quaternion.identity);
                newDoor.name = $"UpDoor-{roomCount}";
            }
        }
        if (direction == Vector2.down) // Open bottom door
        {
            Debug.Log("Bottom door is opening");
            botDoor.SetActive(true);
            if (!botDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(botDoor, botDoor.transform.position, Quaternion.identity);
                newDoor.name = $"BotDoor-{roomCount}";
            }
        }
        if (direction == Vector2.left) // Open left door
        {
            Debug.Log("Left door is opening");
            leftDoor.SetActive(true);
            if (!leftDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(leftDoor, leftDoor.transform.position, Quaternion.identity);
                newDoor.name = $"LeftDoor-{roomCount}";
            }
        }
        if (direction == Vector2.right) // Open right door
        {
            Debug.Log("Right door is opening");
            rightDoor.SetActive(true);
            if (!rightDoor.activeInHierarchy)
            {
                var newDoor = Instantiate(rightDoor, rightDoor.transform.position, Quaternion.identity);
                newDoor.name = $"RightDoor-{roomCount}";
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
