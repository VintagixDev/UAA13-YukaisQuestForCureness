using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public bool isOpen = false;
    public List<GameObject> Items;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.name == "Player" && isOpen == false)
        {

            isOpen = true;
            Vector3 pos = transform.position;
            pos.y += 1;
            pos.x -= 1;
            foreach(GameObject item in Items)
            {
                pos.x += 1;
                GameObject instanciated = Instantiate(item);
                instanciated.transform.position = pos;
                instanciated.name = item.name;
                
            }

        }
    }
}
