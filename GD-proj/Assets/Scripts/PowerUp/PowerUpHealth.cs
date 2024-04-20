using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHealth : PowerUp
{
    protected override void Start()
    {
        SetupEmissiveColor(Color.green);
    }

    protected override void ApplyPowerUp()
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth + 20, 0, 100);
    }
}
