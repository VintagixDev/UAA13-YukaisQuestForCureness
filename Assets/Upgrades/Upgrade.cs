using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Upgrade : MonoBehaviour
{


    public int upgradeID;
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;


    public bool upgradeEffectOnce;
    public bool upgradeHasBeenUsed = false;

    public PlayerStats stats;

    public GameObject UIAnimation;



    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = upgradeSprite;
    }

   

    public virtual void UpgradeAction()
    {
        Debug.Log(upgradeName);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;
        Debug.Log(gameObject);
        stats.playerUpgrades.Add(gameObject);
        gameObject.SetActive(false);
        gameObject.transform.parent = stats.gameObject.transform.Find("Upgrades");
        UpgradeAnimationUI();
    }

    public async void UpgradeAnimationUI()
    {
        
        
        RectTransform rectTransform = UIAnimation.GetComponent<RectTransform>();
        rectTransform.Find("Upgrade Image").GetComponent<Image>().sprite = upgradeSprite;
        rectTransform.Find("Upgrade Name").GetComponent<TextMeshProUGUI>().text = upgradeName;
        rectTransform.Find("Upgrade Description").GetComponent<TextMeshProUGUI>().text = upgradeDescription;

        rectTransform.DOScale(Vector3.zero, 0);
        rectTransform.DOLocalMoveY(0, 0);


        rectTransform.DOShakeRotation(1f, 1f, 7, 90, false);

        rectTransform.DOScale(Vector3.one, 1f);
        rectTransform.DOScale(Vector3.one, 1.5f).SetDelay(1);
        rectTransform.DOLocalMoveY(750, 1.5f).SetEase(Ease.InSine).SetDelay(3f);

        
    }
}
