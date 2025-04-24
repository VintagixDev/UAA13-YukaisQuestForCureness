using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    /// <summary>
    /// timeToDeath : variable qui définit la durée du projectile
    /// </summary>
    [Header("Variables")]
    public float timeToDeath = 1f;
    PlayerMethods playerMethods;
    // Start is called before the first frame update
    void Start()
    {
        playerMethods = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMethods>();
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
