using System;
using TMPro;
using UnityEngine;


public class HealthDisplayScript : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private TextMeshProUGUI HealthText;

    private void Awake()
    {
        HealthText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        HealthText.SetText($"Health: {playerHealth.currentHealth}");
    }
}