using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : Upgrade
{
    public int dmgUpgrade = 2;
    public PlayerStats stats;

    public override void UpgradeAction()
    {
        stats.playerDamage += dmgUpgrade;
        Debug.Log("Damage increase!");
    }
}
