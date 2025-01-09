using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    /// <summary>
    /// speed : Vitesse du joueur
    /// speedDecrease : Sert a diminuer la vitesse du joueur (facteur de glissance)
    /// canMove : Détermine si le joueur peut bouger ou non
    /// </summary>

    [Header("Speed")]
    public float speed = 4.5f;
    public float speedDecrease = 1.025f;
    public bool canMove = true;

    /// <summary>
    /// horizontalVelocity : Vitesse horizontale
    /// verticalVelocity : Vitesse verticale
    /// </summary>

    [Header("velocity axis")]
    public float horizontalVelocity;
    public float verticalVelocity;

    /// <summary>
    /// stats : Sert à la récupération des statistiques du joueur
    /// </summary>

    [Header("Stats")]
    public PlayerStats stats;

    /// <summary>
    /// currentRoom : Référence à la pièce actuelle du joueur
    /// </summary>
    private Room currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        horizontalVelocity = 0;
        verticalVelocity = 0;

        // Initialiser la pièce actuelle
        UpdateCurrentRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.currentRoom != null)
        {
            // Récupérez l'ID de la pièce actuelle
            //if (int.TryParse(stats.currentRoom, out int currentRoomId))
            //{
            //    //Debug.Log($"Le joueur se trouve dans la pièce avec l'ID: {currentRoomId}");
            //}
            //else
            //{
            //    //Debug.LogWarning("Invalid room ID format");
            //}

            //Debug.Log($"Le joueur se trouve dans la pièce avec l'ID: {currentRoomId}");
        }

        // N'oubliez pas d'appeler Movements() si nécessaire
        if (canMove)
        {
            Movements();
        }

        UpdateCurrentRoom();
    }


    /// <summary>
    /// Gère les mouvements du joueur en 2D avec un effet de glissement.
    /// Le joueur peut se déplacer horizontalement avec les touches A (gauche) et D (droite),
    /// ainsi que verticalement avec les touches W (haut) et S (bas).
    /// Si aucune touche n'est enfoncée, la vitesse du joueur diminue progressivement jusqu'à s'arrêter.
    /// </summary>
    private void Movements()
    {
        // HORIZONTAL
        if (Input.GetKey(KeyCode.A))
        {
            if (horizontalVelocity > -1f)
            {
                horizontalVelocity -= .1f;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (horizontalVelocity < 1f)
            {
                horizontalVelocity += .1f;
            }
        }
        else
        {
            if (horizontalVelocity < 0f)
            {
                horizontalVelocity = horizontalVelocity / speedDecrease;
                if (horizontalVelocity > -0.1)
                {
                    horizontalVelocity = 0;
                }
            }
            else if (horizontalVelocity > 0f)
            {
                horizontalVelocity = horizontalVelocity / speedDecrease;
                if (horizontalVelocity < 0.1)
                {
                    horizontalVelocity = 0;
                }
            }
        }

        // VERTICAL
        if (Input.GetKey(KeyCode.W))
        {
            if (verticalVelocity < 1f)
            {
                verticalVelocity += .1f;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (verticalVelocity > -1f)
            {
                verticalVelocity -= .1f;
            }
        }
        else
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = verticalVelocity / speedDecrease;
                if (verticalVelocity > -0.1)
                {
                    verticalVelocity = 0;
                }
            }
            else if (verticalVelocity > 0f)
            {
                verticalVelocity = verticalVelocity / speedDecrease;
                if (verticalVelocity < 0.1)
                {
                    verticalVelocity = 0;
                }
            }
        }

        Vector2 direction = new Vector2(horizontalVelocity, verticalVelocity);
        transform.Translate(direction * speed * stats.playerMoveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Met à jour la pièce actuelle du joueur en fonction de sa position.
    /// </summary>
    private void UpdateCurrentRoom()
    {
        Vector3 playerPosition = transform.position;
        int roomX = Mathf.FloorToInt(playerPosition.x / 10);
        int roomY = Mathf.FloorToInt(playerPosition.y / 10);

        if (currentRoom == null || currentRoom.RoomIndex != new Vector2Int(roomX, roomY))
        {
            Room newRoom = FindRoomAtPosition(roomX, roomY);
            if (newRoom != null)
            {
                currentRoom = newRoom;
                stats.currentRoom = newRoom.RoomID.ToString(); // Met à jour l'ID de la pièce actuelle dans PlayerStats
            }
        }
    }


    /// <summary>
    /// Recherche la pièce à une position donnée dans la grille.
    /// Remplacez cette fonction par votre propre logique pour déterminer la pièce.
    /// </summary>
    private Room FindRoomAtPosition(int x, int y)
    {
        Room[] rooms = FindObjectsOfType<Room>();
        foreach (Room room in rooms)
        {
            if (room.RoomIndex == new Vector2Int(x, y))
            {
                return room;
            }
        }
        return null;
    }
}
