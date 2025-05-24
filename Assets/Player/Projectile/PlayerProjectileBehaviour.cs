using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehaviour : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField, Tooltip("Durée avant que le projectile soit détruit automatiquement")]
    private float _timeToDeath = 1f;
    public float TimeToDeath {  get { return _timeToDeath; } set { _timeToDeath = value; } }

    void Start()
    {
        StartCoroutine(Despawn());
    }

    /// <summary>
    /// Coroutine qui détruit le projectile après un certain temps
    /// </summary>
    /// <returns></returns>
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(_timeToDeath);
        Destroy(gameObject);
    }

    /// <summary>
    /// Gère les collisions avec les autres objets
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // On ignore les collisions avec le joueur ou d'autres projectiles amis/ennemis
        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other);
            Destroy(gameObject);
        } else if (!other.CompareTag("Player") && !other.CompareTag("FriendlyProjectile"))
        {
            Destroy(gameObject);
        }
    }
}
