using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    /// <summary>
    /// timeToDeath : variable qui d�finit la dur�e du projectile
    /// </summary>
    [Header("Variables")]
    public float timeToDeath = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(timeToDeath);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("FriendlyProjectile") && !other.CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
        }

    }
}
