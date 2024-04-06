using System.Collections.Generic;
using UnityEngine;


// TODO: fix after implementing level management
public class GunStatsManager : MonoBehaviour
{
    [SerializeField] private List<GunScriptableObject> guns;

    private void Start()
    {
        foreach (var gun in guns)
        {
            gun.gunAmmoConfig.CurrentAmmo = gun.gunAmmoConfig.MaxAmmoStandard;
            gun.gunAmmoConfig.CurrentClip = gun.gunAmmoConfig.ClipSizeStandard;
            gun.ShootConfig.FireRate = gun.ShootConfig.FireRateStandard;
        }
    }
}