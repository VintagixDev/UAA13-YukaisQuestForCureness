using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changerdetage : MonoBehaviour
{
    [SerializeField] private TeleportationManager teleportationManager; // Script de gestion des déplacement dans le donjon
    [SerializeField] private GameStat gameStat; // Script des stats de partie
    [SerializeField] private GameSupervisor MASTER;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Le joueur a touché la porte.");

            teleportationManager.RecenterPlayer(collision.gameObject);
            gameStat.CurrentFloor = gameStat.CurrentFloor + 1;
            MASTER.GenerateNewFloor();
        }
    }
}
