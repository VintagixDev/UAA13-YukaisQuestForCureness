using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
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
                Debug.Log("La pièce actuelle du joueur est '" + playerStats.currentRoom + "'");
                // Affiche un message dans la console pour indiquer le changement de salle
                //Debug.Log($"Le joueur est maintenant dans la pièce {playerStats.currentRoom}");
            }
        }
        else
        {
            Debug.LogWarning("La porte connectée n'est pas définie.");
        }
    }
}