using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int enemyID;

    [Header("Stats")]
    public int enemyHP;
    public double enemyDmg;
    public float enemyMovementSpeed;
    public bool enemyIsRange;

    [Header("Melee")]
    public double enemyMeleeRange;

    [Header("Ranged")]
    public double enemyProjectileSize = 1;
    public double enemyProjectileReach = 1;
    public double enemyProjectileSpeed = 1;

    [Header("Movements")]
    public Transform player;
    public bool defaultMovement = true;
    public bool isStunned;

    public void Update()
    {

        if (defaultMovement && !isStunned)
        {
            Movements();

        }
    }

    public void Movements()
    {
        
        if (transform.position.x < player.position.x)
        {

            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * enemyMovementSpeed);
        
    }
}



