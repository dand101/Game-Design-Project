using UnityEngine;

namespace SlimUI.ModernMenu
{
    public class CheckMusicVolume : MonoBehaviour
    {
        public void Start()
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicEffectsVolume");
        }

        public void UpdateVolume()
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        }

        public void UpdateEffectsVolume()
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicEffectsVolume");
        }
    }
}