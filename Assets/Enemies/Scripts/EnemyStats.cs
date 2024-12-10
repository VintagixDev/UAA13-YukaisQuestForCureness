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
    public int enemyDmg;
    public float enemyMovementSpeed;
    public bool enemyIsRange;



    [Header("Ranged")]
    public float enemyProjectileSize = 1;
    public float enemyProjectileReach = 1;
    public float enemyProjectileSpeed = 1;
    public int enemyAttackSpeed = 60; // Nombre de frame

    public GameObject enemyProjectile;

    [Header("Movements")]
    public Transform player;
    public bool defaultMovement = true;
    public bool isStunned;

    [Header("Methods")]
    public EnemyMethods methods;
    public void Update()
    {
        if (!isStunned)
        {
            if(defaultMovement) methods.Movements(enemyIsRange);
        }
        
        
    }

    
}



