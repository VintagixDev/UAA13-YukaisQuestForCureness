using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{


    public int upgradeID;
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;
    public bool upgradeStart;


    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = upgradeSprite;
        if (upgradeStart)
        {
            UpgradeAction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!upgradeStart)
        {
            UpgradeAction();

        }
    }

    public virtual void UpgradeAction()
    {
        Debug.Log(upgradeName);
    }

}
