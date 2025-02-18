using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : Upgrade
{
    public int dmgUpgrade = 2;

    public override void UpgradeAction()
    {
        upgradeHasBeenUsed = true;
        stats.playerDamage += dmgUpgrade;
        Debug.Log("Damage increase!");
    }
}
