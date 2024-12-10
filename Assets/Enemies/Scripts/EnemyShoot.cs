using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public EnemyStats stats;
    public PlayerMethods playerMethods;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerMethods = collision.gameObject.GetComponent<PlayerMethods>();
        if (playerMethods != null && stats != null)
        {
            playerMethods.DamagePlayer(stats.enemyDmg);
        }
        

    }
}
