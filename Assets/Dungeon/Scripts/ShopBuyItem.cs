using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyItem : MonoBehaviour
{
    public int price = 3;
    PlayerStats playerStats;
    StatsUI statsUI;
    bool bought = false;

    private void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        statsUI = GameObject.Find("HUD").GetComponent<StatsUI>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameobj = collision.gameObject;
        //Debug.Log(playerStats.playerGolds);
        if (gameobj.CompareTag("Player"))
        {
            if (playerStats.playerGolds >= price && !bought)
            {
                bought = true;
                playerStats.playerHP++;
                playerStats.playerGolds -= price;
                
                Object.Destroy(this.gameObject, 0);
                if(statsUI) statsUI.updateDisplayHearts();
                if (statsUI) statsUI.updateCollectableUI();
            }
        }
    }
}
