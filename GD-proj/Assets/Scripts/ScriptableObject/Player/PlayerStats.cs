using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int C_Health = 100;
    public int Points = 0;
    public bool isReloading = false;
    
    public void AddPoints(int points)
    {
        Points += points;
    }
}