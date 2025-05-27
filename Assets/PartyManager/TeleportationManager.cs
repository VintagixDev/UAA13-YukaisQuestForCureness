using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    [Header("Composantes")]
    [Tooltip("Maitre des lieux")]
    [SerializeField] private GameSupervisor gameSupervisor;
    [Tooltip("Statistiques g�n�rales")]
    [SerializeField] private GameStat gameStat;

    private const float teleportOffset = 2f;

    /// <summary>
    /// T�l�porte un joueur � la position d'une porte connect�e et met � jour son �tat de salle actuelle.
    /// </summary>
    /// <param name="player">Le GameObject repr�sentant le joueur � t�l�porter.</param>
    /// <param name="targetDoor">La porte connect�e cible vers laquelle t�l�porter le joueur.</param>
    public void TeleportPlayer(GameObject player, Door targetDoor, int roomId)
    {
        if (targetDoor == null)
        {
            Debug.LogWarning("Warning 309: No connected door is instantiated for this door.");
            return;
        }

        Vector3 teleportPosition = targetDoor.transform.position;

        // D�calage selon l'orientation
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
    /// Recentre le joueur dans une nouvelle salle (spawn au centre de la pi�ce).
    /// </summary>
    public void RecenterPlayer(GameObject player)
    {
        if (player != null)
        {
            player.transform.position = Vector3.zero;
        }
    }
}
