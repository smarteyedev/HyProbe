using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public static AudioManager instance;


        [SerializeField] private AudioClip hover;
        [SerializeField] private AudioClip pressed;
        [SerializeField] private AudioClip Spawn;
        [SerializeField] private AudioClip popup;
        [SerializeField] private AudioClip typing;

        private void Awake()
        {
            instance = this;
        }


        public void PlayHoverButton()
        {
            audioSource.PlayOneShot(hover);
        }

        public void PlayClickButton()
        {
            audioSource.PlayOneShot(pressed);
        }

        public void PlaySpawnHologram()
        {
            audioSource.PlayOneShot(Spawn);
        }

        public void PlayPopupButon()
        {
            audioSource.PlayOneShot(popup);
        }

        public void PlayTyping()
        {
            audioSource.PlayOneShot(typing);
        }
    }
}
