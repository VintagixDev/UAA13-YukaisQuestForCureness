using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMethods : MonoBehaviour
{

    public PlayerStats stats;
    public StatsUI statsUI;
    // Start is called before the first frame update

    // Update is called once per frame

    void Start()
    {

      

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DamagePlayer(1);
        }
    }

    public void DamagePlayer(int damage)
    {
        stats.playerHP -= damage;
        if(stats.playerHP <= 0) 
        {
            SceneManager.LoadScene("DeathScene");
        }
        statsUI.updateDisplayHearts();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);
        GameObject obj = collision.gameObject;
        switch(obj.name)
        {
            case "Gold":
                Destroy(obj);
                stats.playerGolds++;
                statsUI.updateCollectableUI();
                break;
            case "Key":
                Destroy(obj);
                stats.playerKeys++;
                statsUI.updateCollectableUI();
                break;
            default:
                break;
        }
    }
}
