using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Text))]
public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector GunSelector;
    private Text AmmoText;


    private void Awake()
    {
        AmmoText = GetComponent<Text>();
    }

    private void Update()
    {
        AmmoText.text =
            $"{GunSelector.ActiveGun.gunAmmoConfig.CurrentClip} / {GunSelector.ActiveGun.gunAmmoConfig.CurrentAmmo}";
    }
}