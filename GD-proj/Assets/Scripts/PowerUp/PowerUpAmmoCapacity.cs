using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAmmoCapacity : PowerUp
{
    protected override void Start()
    {
        SetupEmissiveColor(Color.magenta);
    }

    protected override void ApplyPowerUp()
    {
        // increase the ammo capacity
        var ammoConfig = GetPlayerAmmoConfig();
        int newClipSize = ammoConfig.ClipSize + 10;
        ammoConfig.ClipSize = ammoConfig.CurrentClip = newClipSize;
    }
}
