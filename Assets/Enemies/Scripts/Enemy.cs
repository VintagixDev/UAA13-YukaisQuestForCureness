using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int enemyID;

    [Header("Stats")]


    public int enemyHP;
    public int enemyDmg;
    public float enemyMovementSpeed;
    public bool enemyIsRange;



    [Header("Ranged")]
    public float enemyProjectileSize = 1;
    public float enemyProjectileReach = 1;
    public float enemyProjectileSpeed = 1;
    public int enemyAttackSpeed = 60; // Nombre de frame
    public int cdAttackSpeed;

    public GameObject enemyProjectile;

    [Header("Movements")]
    public bool isStunned;

    [Header("Player")]
    public Transform player;
    public PlayerMethods playerMethods;

    public void Start()
    {

        cdAttackSpeed = enemyAttackSpeed;
    }

    private void Update()
    {

        if (isStunned) return;
        Movements();
        if (enemyIsRange)
        {
            cdAttackSpeed--;
            if (cdAttackSpeed == 0)
            {
                cdAttackSpeed = enemyAttackSpeed;
                EnemyRangeAttack();
            }

        }
    }


    public virtual void Movements()
    {
        if (transform.position.x < player.position.x)
        {

            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (!enemyIsRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * enemyMovementSpeed);
        }
        else
        {

            Vector2 pos = new Vector2(transform.position.x, player.position.y);
            transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * enemyMovementSpeed);
            float distance = Vector2.Distance(transform.position, player.position);
            Debug.Log(distance);
        }
    }


    /// <summary>
    /// Si collision avec joueur, Damage player
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Player")
        {
            playerMethods.DamagePlayer(enemyDmg);
        }
    }

    public virtual void EnemyRangeAttack()
    {
        Vector2 pos = player.position - gameObject.transform.position;
        var distance = pos.magnitude;
        var direction = pos / distance;

        GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);

        float currentSize = projectile.transform.localScale.x;
        projectile.transform.localScale = new Vector2(currentSize * enemyProjectileSize, currentSize * enemyProjectileSize);
        projectile.GetComponent<EnemyShoot>().enemyDmg = enemyDmg;
        projectile.GetComponent<EnemyShoot>().playerMethods = playerMethods;

        projectile.GetComponent<Rigidbody2D>().velocity = direction * enemyProjectileSpeed;
        projectile.GetComponent<BulletShoot>().timeToDeath = enemyProjectileReach;
    }
}
