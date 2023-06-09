using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class AudioSettingsUI : MonoBehaviour
    {
        [SerializeField] Slider musicVolume;
        [SerializeField] Slider soundFXVolume;

        string musicVolumeString = "musicVolume";
        string sfxVolumeString = "sfxVolume";
        bool isMuted = false;

        void Awake()
        {
            musicVolume.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
            soundFXVolume.onValueChanged.AddListener(delegate { UpdateSoundVolume(); });

            if (!PlayerPrefs.HasKey(musicVolumeString))
            {
                PlayerPrefs.SetFloat(musicVolumeString, 1);
            }
            if (!PlayerPrefs.HasKey(sfxVolumeString))
            {
                PlayerPrefs.SetFloat(sfxVolumeString, 1);
            }

            MusicManager.Main.SetVolume(PlayerPrefs.GetFloat(musicVolumeString, 0.5f));
            musicVolume.value = PlayerPrefs.GetFloat(musicVolumeString);
            SFXManager.Main.SetVolume(PlayerPrefs.GetFloat(sfxVolumeString, 0.5f));
            soundFXVolume.value = PlayerPrefs.GetFloat(sfxVolumeString);

            isMuted = PlayerPrefs.GetInt("soundMuted") == 1 ? true : false;
            if (isMuted)
            {
                MusicManager.Main.Mute();
                SFXManager.Main.Mute();
            }
            else
            {
                MusicManager.Main.UnMute();
                SFXManager.Main.UnMute();
            }
        }

        void UpdateMusicVolume()
        {
            MusicManager.Main.SetVolume(musicVolume.value);
        }

        void UpdateSoundVolume()
        {
            SFXManager.Main.SetVolume(soundFXVolume.value);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetInt("Mute_FX", isMuted ? 1 : 0);
            PlayerPrefs.SetFloat(musicVolumeString, musicVolume.value);
            PlayerPrefs.SetFloat(sfxVolumeString, soundFXVolume.value);
        }
    }
}
