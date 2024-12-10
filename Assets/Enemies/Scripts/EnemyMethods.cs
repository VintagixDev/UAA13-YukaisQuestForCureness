using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMethods : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerMethods playerMethods;
    public EnemyStats stats;
   
    private int cdAttackSpeed;
    private Vector3 offset;

    public void Start()
    {
        
        cdAttackSpeed = stats.enemyAttackSpeed;
    }

    public void Movements(bool range)
    {

        if (transform.position.x < stats.player.position.x)
        {

            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (range == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, stats.player.position, Time.deltaTime * stats.enemyMovementSpeed);
        }
        else
        {
            
            Vector2 pos = new Vector2(transform.position.x, stats.player.position.y);
            transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * stats.enemyMovementSpeed);
        }

    }

    private void Update()
    {
        
        


        if (stats.enemyIsRange)
        {
            cdAttackSpeed--;
            if(cdAttackSpeed == 0)
            {
                cdAttackSpeed = stats.enemyAttackSpeed;
                EnemyDefaultRangeAttack();
            }
        
        }
    }


    /// <summary>
    /// Si collision avec joueur, Damage player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
   
        if(collision.gameObject.name == "Player")
        {
            
           
            playerMethods.DamagePlayer(stats.enemyDmg);
               
            
            
        }
    }

    private void EnemyDefaultRangeAttack()
    {
        
        
        Vector2 pos = stats.player.position - gameObject.transform.position;
        var distance = pos.magnitude;
        var direction = pos / distance;
         
        GameObject projectile = Instantiate(stats.enemyProjectile, transform.position, Quaternion.identity);

        float currentSize = projectile.transform.localScale.x;
        projectile.transform.localScale = new Vector2(currentSize * stats.enemyProjectileSize, currentSize * stats.enemyProjectileSize);
        projectile.GetComponent<EnemyShoot>().stats = stats;
        projectile.GetComponent<EnemyShoot>().playerMethods = playerMethods;

        projectile.GetComponent<Rigidbody2D>().velocity = direction * stats.enemyProjectileSpeed;
        projectile.GetComponent<BulletShoot>().timeToDeath = stats.enemyProjectileReach;
        
    }

}
