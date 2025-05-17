using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjetile : MonoBehaviour
{
    [Tooltip("Vitesse de déplacement du projectile")]
    [SerializeField] private float _speed = 10f;

    [Tooltip("Dommages infligés au joueur")]
    [SerializeField] private int _damage = 1;

    [Tooltip("Durée de vie avant auto-destruction")]
    [SerializeField] private float _lifetime = 5f;

    private Vector2 _direction;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    public float Lifetime
    {
        get => _lifetime;
        set => _lifetime = value;
    }

    public Vector2 Direction
    {
        get => _direction;
        set => _direction = value.normalized;
    }

    void Start()
    {
        Destroy(gameObject, _lifetime); // Auto-destruction
    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Projectile a touché le joueur !");
            // Appelle une méthode sur le joueur si besoin
            // collision.GetComponent<PlayerHealth>()?.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
