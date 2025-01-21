using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public bool isOpen = false;
    public List<GameObject> Items;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        GameObject gameObject = collision.gameObject;
        if (gameObject.name == "Player" && isOpen == false)
        {
            isOpen = true;


        }
    }
}
