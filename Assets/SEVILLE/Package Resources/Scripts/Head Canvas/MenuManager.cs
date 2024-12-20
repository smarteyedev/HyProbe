using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Tproject.AudioManager;

namespace Seville
{
    public class MenuManager : MonoBehaviour
    {
        public Slider musicSlider, sfxSlider;

        public void SetUpVolume()
        {
            musicSlider.value = AudioManager.Instance.GetMasterVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        }

        public void SetMusicVolume(float value)
        {
            AudioManager.Instance.SetMasterVolume(value);
        }

        public void SetSfxVolume(float value)
        {
            AudioManager.Instance.SetSfxVolume(value);
        }

        public void MuteMusic()
        {
            AudioManager.Instance.MuteMusic();
        }

        public void UnmuteMusic()
        {
            AudioManager.Instance.UnmuteMusic();
        }

        public void QuitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}