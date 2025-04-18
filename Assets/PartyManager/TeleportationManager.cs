using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{

    public GameSupervisor gameSupervisor;
    public GameStat gameStat;
    /// <summary>
    /// T�l�porte un joueur � la position d'une porte connect�e et met � jour son �tat de salle actuelle.
    /// </summary>
    /// <param name="player">Le GameObject repr�sentant le joueur � t�l�porter.</param>
    /// <param name="targetDoor">La porte connect�e cible vers laquelle t�l�porter le joueur.</param>
    /// <remarks>
    /// Si la porte connect�e est null, un avertissement est affich� dans la console.
    /// Si le joueur poss�de un composant <c>PlayerStats</c>, son ID de salle actuelle est mis � jour
    /// en fonction de l'ID de la porte connect�e.
    /// </remarks>
    public void TeleportPlayer(GameObject player, Door targetDoor, int roomId)
    {
        if (targetDoor != null)
        {
            Vector3 teleportPosition = targetDoor.transform.position;

            if (targetDoor._orientation == "N")
            {
                teleportPosition.y -= 2;
            }
            else if (targetDoor._orientation == "E")
            {
                teleportPosition.x -= 2;
            }
            else if (targetDoor._orientation == "S")
            {
                teleportPosition.y += 2;
            }
            else if (targetDoor._orientation == "W")
            {
                teleportPosition.x += 2;
            }

            player.transform.position = teleportPosition;

            gameStat.CurrentRoom = targetDoor._roomId;
            gameSupervisor.Battle();

        }
        else
        {
            Debug.LogWarning("Warning 309: No door connected is instantiated for this door");
        }
    }

    // Recentre le joueur dans une nouvelle salle de spawn � un autre �tage
    public void RecenterPlayer(GameObject player)
    {
        Vector3 teleportPosition = player.transform.position;
        teleportPosition.x = 0;
        teleportPosition.y = 0;
        player.transform.position = teleportPosition;
    }
}