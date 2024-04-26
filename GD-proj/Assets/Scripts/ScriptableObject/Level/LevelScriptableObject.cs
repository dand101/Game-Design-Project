using UnityEngine;

[CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "Level/LevelScriptableObject")]
public class LevelScriptableObject : ScriptableObject
{
    public int currentLevel = 1;

    public void CompleteLevel()
    {
        currentLevel++;
    }

    public void ResetProgress()
    {
        currentLevel = 1;
    }
}