using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{

    public GameObject player;
    public Vector3 playerPos;

    private Vector3 direction; // Direction vers le joueur

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerPos = player.transform.position;

        // calcul d'angle
        direction = (playerPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Appliquer la rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        GetComponent<SpriteRenderer>().flipY = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().DamagePlayer(1);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("FriendlyProjectile"))
        {
            Destroy(gameObject);
        }
    }
}
