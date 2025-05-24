using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeadfulBossScript : Boss
{

    public GameObject projectile;

    public PlayerStats playerStats;
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("FriendlyProjectile"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerStats = player.GetComponent<PlayerStats>();
            Destroy(collision.gameObject);
            TakeDamage(playerStats.playerDamage);
        }
    }

    void Start()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (bossCurrentHP > 0) 
        {

            GameObject currentProjectile = Instantiate(projectile);
            currentProjectile.transform.position = transform.position;
            StartCoroutine(DestroyBullet(currentProjectile));
            yield return new WaitForSeconds(1f);
            
        }
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(2);
        Destroy(bullet);
        
    }

}
