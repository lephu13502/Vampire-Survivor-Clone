using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPassiveItem : PassiveItems
{
    protected override void ApplyModifier()
    {
        player.currentMoveSpeed *= 1 + passiveItemsData.Multiplier / 100f;
    }
}
