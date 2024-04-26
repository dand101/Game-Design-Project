using System;
using UnityEngine;


public class LevelManagement : MonoBehaviour
{
    public LevelScriptableObject LevelScriptableObject;
    public GunStatsManager gunStatsManager;


    private void Start()
    {
        if (LevelScriptableObject == null)
        {
            Debug.LogError("LevelScriptableObject is not set in the LevelManagement script. wtf");
        }

        if (gunStatsManager == null)
        {
            Debug.LogError("GunStatsManager is not set in the LevelManagement script. what 2");
        }

        if (LevelScriptableObject.currentLevel == 1)
        {
            gunStatsManager.setAllGunsStart();
        }
        else
        {
            gunStatsManager.keepGunStats();
        }
    }

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