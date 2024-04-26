using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFireRate : PowerUp
{
    protected override void Start()
    {
        SetupEmissiveColor(Color.red);
    }

    protected override void ApplyPowerUp()
    {
        var gunConfig = GetPlayerGunConfig();
        gunConfig.FireRate /= 1.2f;
    }
}
