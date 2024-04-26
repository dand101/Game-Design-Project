using System;
using UnityEngine;


public class LevelManagement : MonoBehaviour
{
    public LevelScriptableObject LevelScriptableObject;
    public PlayerStats playerStats;
    public GunStatsManager gunStatsManager;
    public LvlCompleteDisplay lvlCompleteDisplay;
    public GameObject upgradesGameObject;
    public float upgradesSpawnDistance = 5f;


    private void Start()
    {
        if (LevelScriptableObject == null)
        {
            Debug.LogError("LevelScriptableObject is not set in the LevelManagement script. what");
        }

        if (gunStatsManager == null)
        {
            Debug.LogError("GunStatsManager is not set in the LevelManagement script. what 2");
        }

        if (LevelScriptableObject.currentLevel == 1)
        {
            playerStats.C_Health = 100;
            playerStats.Points = 0;
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
        lvlCompleteDisplay.DisplayLevelComplete();
        //Debug.Log("Level completed. Current level: " + LevelScriptableObject.currentLevel);

        GameObject playerObj = GameObject.Find("PlayerObj");

        if (playerObj != null)
        {
            Vector3 spawnPosition =
                playerObj.transform.position + (playerObj.transform.forward * upgradesSpawnDistance);

            if (upgradesGameObject != null)
            {
                Instantiate(upgradesGameObject, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Upgrades GameObject is not assigned in the LevelManagement script.");
            }
        }
        else
        {
            Debug.LogError(
                "PlayerObj GameObject not found. Make sure the GameObject with the name 'PlayerObj' exists in the scene.");
        }
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