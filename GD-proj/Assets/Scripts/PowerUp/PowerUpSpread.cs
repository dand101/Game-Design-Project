using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpread : PowerUp
{
    protected override void Start()
    {
        SetupEmissiveColor(Color.cyan);
    }

    protected override void ApplyPowerUp()
    {
        var gunConfig = GetPlayerGunConfig();
        gunConfig.Spread *= 2f;
    }
}
