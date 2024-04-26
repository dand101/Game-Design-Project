using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAmmoIncrease : PowerUp
{
    protected override void Start()
    {
        SetupEmissiveColor(Color.yellow);
    }

    protected override void ApplyPowerUp()
    {
        // increase the current ammo
        var ammoConfig = GetPlayerAmmoConfig();
        ammoConfig.CurrentAmmo += 50;
    }
}
