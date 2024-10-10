using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Yukai/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int enemyID;

    [Header("Stats")] 
    public int enemyHP;
    public double enemyDmg;
    public float enemyMovementSpeed;
    public Sprite enemySprite;
    public bool enemyIsRange;

    [Header("Ranged")]
    public double enemyProjectileSize = 1;
    public double enemyProjectileReach = 1;
    public double enemyProjectileSpeed = 1;


    public Vector3 enemyScale;

}
