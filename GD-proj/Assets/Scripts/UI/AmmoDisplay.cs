using System;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector GunSelector;
    private TextMeshProUGUI AmmoText;


    private void Awake()
    {
        AmmoText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        AmmoText.SetText(
            $"{GunSelector.ActiveGun.gunAmmoConfig.CurrentClip} / {GunSelector.ActiveGun.gunAmmoConfig.CurrentAmmo}");
    }
}