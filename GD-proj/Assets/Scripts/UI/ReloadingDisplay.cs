using System;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class ReloadingDisplay : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private TextMeshProUGUI ReloadingText;


    private void Awake()
    {
        ReloadingText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (playerStats.isReloading)
        {
            ReloadingText.text = "Reloading...";
        }
        else
        {
            ReloadingText.text = "";
        }
    }
}