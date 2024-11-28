using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMethods : MonoBehaviour
{


    public int iFrames = 160;
    private int cdTime;
    public PlayerStats stats;
    public StatsUI statsUI;
    // Start is called before the first frame update

    // Update is called once per frame

    void Start()
    {
        cdTime = iFrames;


    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.U))
        {
            DamagePlayer(1);
        }

        if (cdTime < iFrames)
        {
            cdTime--;
            if (cdTime == 0)
            {
                cdTime = iFrames;
            }
        }
    }
     
    public void DamagePlayer(int damage)
    {
        if (cdTime == iFrames)
        {

            stats.playerHP -= damage;
            if(stats.playerHP <= 0) 
            {
                SceneManager.LoadScene("DeathScene");
            }
            statsUI.updateDisplayHearts();
            cdTime--;
        }
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
