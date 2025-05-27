using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField, Tooltip("Script des stats de parties")]
    private GameStat gameStat;
    public bool isOpen = false;
    public List<GameObject> Items;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        gameStat = GameObject.FindGameObjectWithTag("GameStat").GetComponent<GameStat>();
        GameObject gameObject = collision.gameObject;
        if (gameObject.name == "Player" && !isOpen)
        {
            Debug.Log("Coffre Ouvert");
            isOpen = true;
            Vector3 pos = transform.position;
            pos.y += 1;
            pos.x -= 2;
            foreach(GameObject item in Items)
            {
                pos.x += 1;
                GameObject instanciated = Instantiate(item);
                instanciated.transform.position = pos;
                instanciated.name = item.name;
                
            }
            gameStat.ChestsOpened++;
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            Destroy(this.gameObject, 5f);
        }   
    }
}
