using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMethods : MonoBehaviour
{
    [Header("")]
    [SerializeField] public int iFrames = 160;
    [SerializeField] private int cdTime;
    [SerializeField] public PlayerStats stats;
    [SerializeField] public StatsUI statsUI;

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

        // Upgrade effects
        foreach(GameObject upgrade in stats.playerUpgrades)
        {
            Upgrade upg = upgrade.GetComponent<Upgrade>();
            if (!upg.upgradeEffectOnce || !upg.upgradeHasBeenUsed)
            {
                upg.UpgradeAction();
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
