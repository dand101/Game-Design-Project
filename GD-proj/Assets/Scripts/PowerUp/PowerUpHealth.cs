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
        //Debug.Log("Applying power up health");
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.Heal(20);
    }
}