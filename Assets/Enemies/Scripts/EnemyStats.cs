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

    [Header("Methods")]
    public EnemyMethods methods;
    public void Update()
    {
        if (!isStunned)
        {
            if(defaultMovement) methods.Movements();
        }
        
        
    }

    
}



