using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [Tooltip("The damage dealt by the enemy when it hits the player.")]
    public int enemyDmg;  

    [Tooltip("Speed at which the enemy attacks (higher means faster attacks).")]
    public int enemyAttackSpeed = 60; 
    protected int cdAttackSpeed;  

    [Header("Projectile Settings")]
    [Tooltip("Size factor of the projectile when instantiated.")]
    public float enemyProjectileSize = 1; 

    [Tooltip("Maximum reach of the projectile, after which it disappears.")]
    public float enemyProjectileReach = 1; 

    [Tooltip("Speed at which the projectile travels.")]
    public float enemyProjectileSpeed = 1;  

    [Tooltip("Prefab of the enemy's projectile.")]
    public GameObject enemyProjectile;  

    [Header("Player Interaction")]
    [Tooltip("Reference to the player's transform.")]
    public Transform player;

    [Tooltip("Reference to the player's methods for interacting with the player.")]
    public PlayerMethods playerMethods; 

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMethods = player.GetComponent<PlayerMethods>();  // Récupération des méthodes du joueur
        cdAttackSpeed = enemyAttackSpeed;  
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void EnemyRangeAttack()
    {
        Vector2 pos = player.position - transform.position;
        var distance = pos.magnitude;
        var direction = pos / distance;

        GameObject projectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);

        float currentSize = projectile.transform.localScale.x;
        projectile.transform.localScale = new Vector2(currentSize * enemyProjectileSize, currentSize * enemyProjectileSize);

        var projectileBehaviour = projectile.GetComponent<EnemyProjectileBehaviour>();
        if (projectileBehaviour != null)
        {
            projectileBehaviour.Initialize(enemyDmg, playerMethods, direction * enemyProjectileSpeed, enemyProjectileReach);
        }
    }
}
