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
                teleportPosition.y -= 2; // Diminue l'axe Y pour l'orientation "N"
            }
            else if (targetDoor.orientation == "E")
            {
                teleportPosition.x -= 2; // Diminue l'axe X pour l'orientation "E"
            } else if (targetDoor.orientation == "S")
            {
                teleportPosition.y += 2; // Augmente l'axe Y pour l'orientation "S"
            } else if (targetDoor.orientation == "W")
            {
                teleportPosition.x += 2; // Augmente l'axe X pour l'orientation "W"
            }
            //Vector3 teleportPosition = targetDoor.transform.position;
            player.transform.position = teleportPosition;

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.currentRoom = targetDoor.id.Split('_')[1];
                Debug.Log("La pi�ce actuelle du joueur est '" + playerStats.currentRoom + "'");
                // Affiche un message dans la console pour indiquer le changement de salle
                //Debug.Log($"Le joueur est maintenant dans la pi�ce {playerStats.currentRoom}");
            }
        }
        else
        {
            Debug.LogWarning("La porte connect�e n'est pas d�finie.");
        }
    }
}