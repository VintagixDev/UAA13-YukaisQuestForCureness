using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
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
    public void TeleportPlayer(GameObject player, Door targetDoor)
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
            //ChangeCurrentRoom(player, targetDoor);
            GameStat playerStats = player.GetComponent<GameStat>();
            if (playerStats != null)
            {
                playerStats.CurrentRoom = targetDoor.id.Split('_')[1];
            }
        }
        else
        {
            Debug.LogWarning("Warning 309: No door connected is instantiated for this door");
        }
    }

    public void ChangeCurrentRoom(GameObject player, Door targetDoor)
    {
        GameStat playerStats = player.GetComponent<GameStat>();
        if (playerStats != null)
        {
            playerStats.CurrentRoom = targetDoor.id.Split('_')[1];
        }
    }
}