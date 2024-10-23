using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject botDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    public Vector2Int RoomIndex { get; set; }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)      // Open top door
        {
            topDoor.SetActive(true);
        }
        else if (direction == Vector2Int.down) // Open bottom door
        {
            botDoor.SetActive(true);
        }
        else if (direction == Vector2Int.left) // Open left door
        {
            leftDoor.SetActive(true);
        }
        else if (direction == Vector2Int.right) // Open right door
        {
            rightDoor.SetActive(true);
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
