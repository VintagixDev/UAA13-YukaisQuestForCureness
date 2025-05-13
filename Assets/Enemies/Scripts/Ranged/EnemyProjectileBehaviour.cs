using UnityEngine;

public class EnemyProjectileBehaviour : MonoBehaviour
{
    private int damage;
    private PlayerMethods playerMethods;
    private Vector2 velocity;
    private float timeToDeath;

    public void Initialize(int dmg, PlayerMethods methods, Vector2 vel, float lifetime)
    {
        damage = dmg;
        playerMethods = methods;
        velocity = vel;
        timeToDeath = lifetime;
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
        Destroy(gameObject, timeToDeath);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerMethods == null)
                playerMethods = collision.GetComponent<PlayerMethods>();

            if (playerMethods != null)
                playerMethods.DamagePlayer(damage);

            Destroy(gameObject);
        }
    }
}
