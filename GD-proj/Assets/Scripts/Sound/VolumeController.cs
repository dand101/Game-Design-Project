using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public AudioSource audioSource;
    public string volumePlayerPrefsKey = "MusicVolume";
    
    void Start()
    {
        float volume = PlayerPrefs.GetFloat(volumePlayerPrefsKey, 0.5f); 
        
        audioSource.volume = volume;
    }
}
