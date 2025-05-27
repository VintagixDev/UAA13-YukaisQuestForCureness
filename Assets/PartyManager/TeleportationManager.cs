using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    [Header("Composantes")]
    [Tooltip("Maitre des lieux")]
    [SerializeField] private GameSupervisor gameSupervisor;
    [Tooltip("Statistiques générales")]
    [SerializeField] private GameStat gameStat;

    private const float teleportOffset = 2f;

    /// <summary>
    /// Téléporte un joueur à la position d'une porte connectée et met à jour son état de salle actuelle.
    /// </summary>
    /// <param name="player">Le GameObject représentant le joueur à téléporter.</param>
    /// <param name="targetDoor">La porte connectée cible vers laquelle téléporter le joueur.</param>
    public void TeleportPlayer(GameObject player, Door targetDoor, int roomId)
    {
        if (targetDoor == null)
        {
            Debug.LogWarning("Warning 309: No connected door is instantiated for this door.");
            return;
        }

        Vector3 teleportPosition = targetDoor.transform.position;

        // Décalage selon l'orientation
        switch (targetDoor.Orientation)
        {
            case "N":
                teleportPosition.y -= teleportOffset;
                break;
            case "S":
                teleportPosition.y += teleportOffset;
                break;
            case "E":
                teleportPosition.x -= teleportOffset;
                break;
            case "W":
                teleportPosition.x += teleportOffset;
                break;
            default:
                Debug.LogWarning($"Unknown orientation: {targetDoor.Orientation}");
                break;
        }

        player.transform.position = teleportPosition;

        if (gameStat != null)
        {
            gameStat.CurrentRoom = targetDoor.RoomID;
        }
        else
        {
            //.LogWarning("gameStat reference is missing.");
        }

        if (gameSupervisor != null)
        {
            gameSupervisor.Battle();
        }
        else
        {
            //Debug.LogWarning("gameSupervisor reference is missing.");
        }
    }

    /// <summary>
    /// Recentre le joueur dans une nouvelle salle (spawn au centre de la pièce).
    /// </summary>
    public void RecenterPlayer(GameObject player)
    {
        if (player != null)
        {
            player.transform.position = Vector3.zero;
        }
    }
}
