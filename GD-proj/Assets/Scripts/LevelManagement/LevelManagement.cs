using UnityEngine;


public class LevelManagement : MonoBehaviour
{
    public LevelScriptableObject LevelScriptableObject;
    
    public void CompleteLevel()
    {
        LevelScriptableObject.CompleteLevel();
        Debug.Log("Level completed. Current level: " + LevelScriptableObject.currentLevel);
    }
    
    public void ResetProgress()
    {
        LevelScriptableObject.ResetProgress();
    }
    
    public int GetCurrentLevel()
    {
        return LevelScriptableObject.currentLevel;
    }
}