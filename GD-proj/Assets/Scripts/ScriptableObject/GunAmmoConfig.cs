using UnityEngine;


[CreateAssetMenu(fileName = "AmmoConfig", menuName = "Guns/Ammo", order = 6)]
public class GunAmmoConfig : ScriptableObject
{
    public int MaxAmmo = 300;
    public int ClipSize = 30;

    public int CurrentAmmo = 300;
    public int CurrentClip = 30;

    public void Reload()
    {
        if (CurrentAmmo < ClipSize)
        {
            CurrentClip = CurrentAmmo;
            CurrentAmmo = 0;
            return;
        }
        else
        {
            CurrentAmmo -= ClipSize;
            CurrentClip = ClipSize;
        }
    }

    public bool CanReload()
    {
        return CurrentClip < ClipSize && CurrentAmmo > 0 && CurrentAmmo - ClipSize >= 0;
    }
}