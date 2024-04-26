using TMPro;
using UnityEngine;


public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private TextMeshProUGUI PointsText;

    private void Awake()
    {
        PointsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        PointsText.SetText($"Points: {playerStats.Points}");
    }
}