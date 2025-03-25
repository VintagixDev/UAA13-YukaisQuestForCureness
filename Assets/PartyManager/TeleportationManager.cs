using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{

    GameSupervisor gameSupervisor;
    void Start()
    {
        gameSupervisor = GameObject.Find("GameSupervisor").GetComponent<GameSupervisor>();
    }
    /// <summary>
    /// Téléporte un joueur à la position d'une porte connectée et met à jour son état de salle actuelle.
    /// </summary>
    /// <param name="player">Le GameObject représentant le joueur à téléporter.</param>
    /// <param name="targetDoor">La porte connectée cible vers laquelle téléporter le joueur.</param>
    /// <remarks>
    /// Si la porte connectée est null, un avertissement est affiché dans la console.
    /// Si le joueur possède un composant <c>PlayerStats</c>, son ID de salle actuelle est mis à jour
    /// en fonction de l'ID de la porte connectée.
    /// </remarks>
    public void TeleportPlayer(GameObject player, Door targetDoor, int roomId)
    {
        if (targetDoor != null)
        {
            Vector3 teleportPosition = targetDoor.transform.position;

            if (targetDoor.orientation == "N")
            {
                teleportPosition.y -= 2; 
            } else if (targetDoor.orientation == "E")
            {
                teleportPosition.x -= 2; 
            } else if (targetDoor.orientation == "S")
            {
                teleportPosition.y += 2; 
            } else if (targetDoor.orientation == "W")
            {
                teleportPosition.x += 2; 
            }
            player.transform.position = teleportPosition;

            GameObject gameManager = GameObject.Find("Stats");
            if (gameManager != null)
            {
                GameStat stats = gameManager.GetComponent<GameStat>();
                if (stats != null)
                {
                    //stats.CurrentRoom = targetDoor.roomId;
                    gameSupervisor.Battle();
                    stats.CurrentRoom = roomId;
                }
            }
            else
            {
                Debug.LogWarning("Stats introuvable !");
            }

        }
        else
        {
            Debug.LogWarning("Warning 309: No door connected is instantiated for this door");
        }
    }

    // Recentre le joueur dans une nouvelle salle de spawn à un autre étage
    public void RecenterPlayer(GameObject player)
    {
        Vector3 teleportPosition = player.transform.position;
        teleportPosition.x = 0;
        teleportPosition.y = 0;
        player.transform.position = teleportPosition;
    }
}