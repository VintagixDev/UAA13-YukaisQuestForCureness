using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{

    public GameObject player;
    public Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (transform.position - playerPos).normalized;
        transform.Translate(-dir * Time.deltaTime * 5);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().DamagePlayer(1);
            Destroy(gameObject);
        }
        
    }
}

