using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType Gun;
    [SerializeField] private Transform gunParent;
    [SerializeField] private List<GunScriptableObject> guns;

    [Space] [Header("Runtime Filled")] public GunScriptableObject ActiveGun;
    [Space] [Header("Runtime Filled")] public int activeGunIndex;


    private void Start()
    {
        ActiveGun = guns[0];
        ActiveGun.Spawn(gunParent, this);
        activeGunIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && activeGunIndex != 0)
            SwitchGun(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && activeGunIndex != 1)
            SwitchGun(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) && activeGunIndex != 2) SwitchGun(2);
    }

    private void SwitchGun(int index)
    {
        if (index >= 0 && index < guns.Count)
        {
            var lastGun = ActiveGun;
            ActiveGun = guns[index];
            ActiveGun.Spawn(gunParent, this);
            lastGun.Despawn();
            activeGunIndex = index;
        }
        else
        {
            Debug.LogWarning("Invalid gun index.");
        }
    }
}