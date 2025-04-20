using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public int enemyDmg;
    public PlayerMethods playerMethods;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si la cible est bien le joueur (par le tag)
        if (collision.CompareTag("Player"))
        {
            if (playerMethods == null)
            {
                playerMethods = collision.GetComponent<PlayerMethods>();
            }

            if (playerMethods != null)
            {
                playerMethods.DamagePlayer(enemyDmg);
            }

            Destroy(gameObject); // Le projectile dispara�t apr�s impact
        }
    }
}
