using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private Text PointsText;

    private void Awake()
    {
        PointsText = GetComponent<Text>();
    }

    private void Update()
    {
        PointsText.text = $"Score: {playerStats.Points}";
    }
}