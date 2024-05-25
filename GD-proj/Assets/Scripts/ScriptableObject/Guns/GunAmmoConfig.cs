using UnityEngine;


[CreateAssetMenu(fileName = "AmmoConfig", menuName = "Guns/Ammo", order = 6)]
public class GunAmmoConfig : ScriptableObject
{
    public int MaxAmmo = 300;
    public int ClipSize = 30;

    public int CurrentAmmo = 300;
    public int CurrentClip = 30;
    
    public float ReloadTime = 1.5f;
    public AudioClip reloadSound;

    [Space] [Header("Standard Values for weapon")]
    public int MaxAmmoStandard = 300;

    public int ClipSizeStandard = 30;

   


    
    private void PlayReloadSound(GameObject Model)
    {
        if (reloadSound != null)
        {
            AudioSource.PlayClipAtPoint(reloadSound, Model.transform.position);
        }
        else
        {
            Debug.LogWarning("Reload sound is not assigned in GunScriptableObject.");
        }
    }

    public void Reload(GameObject model)
    {
        PlayReloadSound(model);
        if (CurrentAmmo < ClipSize)
        {
            CurrentClip = CurrentAmmo;
            CurrentAmmo = 0;
        }
        else
        {
            CurrentAmmo -= ClipSize;
            CurrentClip = ClipSize;
        }
    }

    public bool CanReload()
    {
        return CurrentClip < ClipSize && CurrentAmmo > 0;
    }
}