using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public EnemyStats stats;
    public PlayerMethods playerMethods;


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.name == "Player")
        {
            playerMethods.DamagePlayer(1);
        }
    }
}
