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

    // Sert à ouvrir les portes
    public void OpenDoor(Vector2Int direction)
    {
        // Haut
        if (direction == Vector2Int.up)
        {
            topDoor.SetActive(true);
        }
        // Bas
        if (direction == Vector2Int.down)
        {
            botDoor.SetActive(true);
        }
        // Gauche
        if (direction == Vector2Int.left)
        {
            leftDoor.SetActive(true);
        }
        // Droite
        if (direction == Vector2Int.right)
        {
            rightDoor.SetActive(true);
        }
    }
}
