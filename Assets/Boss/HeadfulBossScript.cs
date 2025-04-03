using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadfulBossScript : Boss
{

    public GameObject projectile;
    public PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FriendlyProjectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(playerStats.playerDamage);
        }
    }

}
