using UnityEngine;

public class Snail : RangedEnemy, IStunnable, IEnemyMovement
{
    [Tooltip("Indicateur si l'ennemi est actuellement étourdi")]
    public bool isStunned { get; set; }

    [Header("Snail Movement & Attack")]
    [Tooltip("Référence au joueur pour suivre sa position")]
    public Transform player;

    [Tooltip("Vitesse de déplacement de l'ennemi")]
    public float enemyMovementSpeed;

    [Header("Room Information")]
    [Tooltip("Identifiant de la salle où l'ennemi se trouve")]
    public int roomID;


    private void Start()
    {
        isStunned = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cdAttackSpeed = enemyAttackSpeed;
    }

    private void Update()
    {
        if (isStunned)
            return;

        Movements();

        if (cdAttackSpeed <= 0)
        {
            cdAttackSpeed = enemyAttackSpeed;
            EnemyRangeAttack();
        }
        else
        {
            cdAttackSpeed--;
        }
    }

    public void Movements()
    {
        if (player != null)
        {
            Vector2 pos = new Vector2(transform.position.x, player.position.y);
            transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * enemyMovementSpeed);

            if (transform.position.x < player.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    public void Stun(float duration)
    {
        isStunned = true;  
        Invoke(nameof(EndStun), duration);  
    }

    private void EndStun()
    {
        isStunned = false;  
    }
}
