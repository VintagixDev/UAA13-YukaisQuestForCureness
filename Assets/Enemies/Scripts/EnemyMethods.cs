using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMethods : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerMethods playerMethods;
    public EnemyStats stats;
    private int cooldown = 60;
    private int cdTime = 60;

    public void Movements()
    {

        if (transform.position.x < stats.player.position.x)
        {

            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, stats.player.position, Time.deltaTime * stats.enemyMovementSpeed);

    }

    private void Update()
    {
        Debug.Log(cdTime);
        if (cdTime < cooldown)
        {
            cdTime--;
            if (cdTime == 0)
            {
                cdTime = cooldown;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            
            if(cdTime == cooldown)
            {
                playerMethods.DamagePlayer(stats.enemyDmg);
                cdTime--;
            }
            
            
        }
    }

}
