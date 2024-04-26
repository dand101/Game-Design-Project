using System.Collections.Generic;
using UnityEngine;


public class GunStatsManager : MonoBehaviour
{
    [SerializeField] private List<GunScriptableObject> guns;

    public void setAllGunsStart()
    {
        foreach (var gun in guns)
        {
            gun.gunAmmoConfig.MaxAmmo = gun.gunAmmoConfig.MaxAmmoStandard;
            gun.gunAmmoConfig.ClipSize = gun.gunAmmoConfig.ClipSizeStandard;
            gun.ShootConfig.FireRate = gun.ShootConfig.FireRateStandard;

            gun.gunAmmoConfig.CurrentAmmo = gun.gunAmmoConfig.MaxAmmoStandard;
            gun.gunAmmoConfig.CurrentClip = gun.gunAmmoConfig.ClipSizeStandard;
            gun.ShootConfig.FireRate = gun.ShootConfig.FireRateStandard;
        }
    }

    public void keepGunStats()
    {
        foreach (var gun in guns)
        {
            // no
            // gun.gunAmmoConfig.CurrentAmmo = gun.gunAmmoConfig.MaxAmmo;
            // gun.gunAmmoConfig.CurrentClip = gun.gunAmmoConfig.ClipSize;
            // gun.ShootConfig.FireRate = gun.ShootConfig.FireRate;
        }
    }
}