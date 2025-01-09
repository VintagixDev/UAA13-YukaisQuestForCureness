using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected int enemyID;

    [Header("Stats")]


    protected int enemyHP;
    protected int enemyDmg;
    protected float enemyMovementSpeed;
    protected bool enemyIsRange;



    [Header("Ranged")]
    protected float enemyProjectileSize = 1;
    protected float enemyProjectileReach = 1;
    protected float enemyProjectileSpeed = 1;
    protected int enemyAttackSpeed = 60; // Nombre de frame

    protected GameObject enemyProjectile;

    [Header("Movements")]
    protected Transform player;
    protected bool defaultMovement = true;
    protected bool isStunned;

    [Header("Methods")]
    protected EnemyMethods methods;

}
