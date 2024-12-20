using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tproject.AudioManager
{
    public class SfxHandler : MonoBehaviour
    {
        private AudioManager audioManager;
        public Sound[] sfxClips;

        public bool isPlayOnStart;
        public string firstClipName;

        // Start is called before the first frame update
        void Start()
        {
            if (AudioManager.Instance != null) audioManager = AudioManager.Instance;
            else Debug.LogWarning("please add Audio Manager for the audio video output");
            if (isPlayOnStart) PlaySfxClip(firstClipName);
        }

        public void PlaySfxClip(string clipName)
        {
            Sound sound = audioManager.FindSound(clipName, sfxClips);
            if (sound != null)
            {
                audioManager.PlayMandatorySFX(sound.clip);
            }
        }

        public void StopSfx()
        {
            audioManager.sfxSource.Stop();
        }
    }
}