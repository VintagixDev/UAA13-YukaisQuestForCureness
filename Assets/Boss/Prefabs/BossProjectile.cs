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
}

