using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinachPassiveItem : PassiveItems
{
    protected override void ApplyModifier()
    {
        player.CurrentMight *= 1 + passiveItemsData.Multiplier / 100f;
    }
}
