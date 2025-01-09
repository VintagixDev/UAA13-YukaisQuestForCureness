using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public bool isOpen = false;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        GameObject gameObject = collision.gameObject;
        if(gameObject.name == "Player")
        {
            isOpen = true;
        }
    }
}
