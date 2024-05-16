using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HealthDisplayScript : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private Text HealthText;

    private void Awake()
    {
        HealthText = GetComponent<Text>();
        HealthText = GetComponent<Text>();
    }

    private void Update()
    {
        HealthText.text = $"Health: {playerHealth.currentHealth}";
    }
}