using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : Upgrade
{

    public StatsUI ui;
    public override void UpgradeAction()
    {
        upgradeHasBeenUsed = true;
        stats.playerHP++;
        ui.updateDisplayHearts();
        
    }
}
