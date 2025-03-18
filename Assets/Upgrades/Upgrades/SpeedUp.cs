using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Upgrade
{

    public float speedMultiplier;
    public override void UpgradeAction()
    {
        upgradeHasBeenUsed = true;
        stats.playerMoveSpeed = stats.playerMoveSpeed * speedMultiplier;

    }
}
