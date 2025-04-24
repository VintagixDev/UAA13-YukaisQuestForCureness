using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyItem : MonoBehaviour
{
    public int price = 3;
    PlayerStats playerStats;
    StatsUI statsUI;

    private void Start()
    {
        PlayerStats playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        statsUI = GameObject.FindWithTag("UI").GetComponent<StatsUI>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Player"))
        {
            if(playerStats.playerGolds >= price)
            {
                playerStats.playerHP++;
                playerStats.playerGolds -= price;
                statsUI.updateDisplayHearts();
            }
        }
    }
}
