using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [Header("Variables")]
    [Tooltip("Durée avant que le projectile soit détruit automatiquement")]
    public float _timeToDeath = 1f;

    //private PlayerController playerController;
    void Start()
    {
        StartCoroutine(Despawn());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(_timeToDeath);
        Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // On ignore les collisions avec le joueur ou d'autres projectiles amis/ennemis
        if (!other.CompareTag("Player") && !other.CompareTag("FriendlyProjectile") && !other.CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
        }
    }
}
