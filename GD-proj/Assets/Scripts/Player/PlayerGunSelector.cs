using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType Gun;
    [SerializeField] private Transform gunParent;
    [SerializeField] private List<GunScriptableObject> guns;

    [Space] [Header("Runtime Filled")] public GunScriptableObject ActiveGun;

    // private void Start()
    // {
    //     GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun);
    //
    //     if (gun == null)
    //     {
    //         Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
    //         return;
    //     }
    //
    //     ActiveGun = gun;
    //     gun.Spawn(GunParent, this);
    // }

    private void Start()
    {
        ActiveGun = guns[0];
        ActiveGun.Spawn(gunParent, this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGun(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && guns.Count >= 2)
        {
            SwitchGun(1);
        }
    }

    private void SwitchGun(int index)
    {
        if (index >= 0 && index < guns.Count)
        {

            GunScriptableObject lastGun = ActiveGun;
            ActiveGun = guns[index];
            ActiveGun.Spawn(gunParent, this);
            lastGun.Despawn();
        }
        else
        {
            Debug.LogWarning("Invalid gun index.");
        }
    }
}