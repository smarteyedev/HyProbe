using System.Collections;
using UnityEngine;

namespace Tproject.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public Sound[] musicSounds, sfxSounds;
        public AudioSource musicSource, sfxSource, videoSource;

        private Coroutine fadeCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            PlayBackgroundMusic("Theme");
        }

        public float GetMasterVolume()
        {
            return musicSource.volume;
        }

        public float GetSFXVolume()
        {
            return sfxSource.volume;
        }

        public void SetMasterVolume(float volume)
        {
            musicSource.volume = volume;
            videoSource.volume = volume;
        }

        public void PlayBackgroundMusic(string name)
        {
            Sound sound = FindSound(name, musicSounds);
            if (sound != null)
            {
                musicSource.clip = sound.clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void PlaySFX(string name)
        {
            Sound sound = FindSound(name, sfxSounds);
            if (sound != null)
            {
                sfxSource.PlayOneShot(sound.clip);
            }
        }

        public void PlayMandatorySFX(AudioClip clip)
        {
            if (clip != null && !sfxSource.mute)
            {
                StartCoroutine(TransitionMandatorySfx(clip, .6f));
            }
        }

        public void SetSfxVolume(float volume) =>
            sfxSource.volume = volume;

        public Sound FindSound(string name, Sound[] sounds)
        {
            foreach (var sound in sounds)
            {
                if (sound.name == name)
                {
                    return sound;
                }
            }

            Debug.LogWarning($"Seville Audio Manager: {name} isn't available");
            return null;
        }

        public void MuteMusic()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeOutMusic(.5f));
        }

        public void UnmuteMusic()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeInMusic(1f));
        }

        public void StartTransitionToNewMusic(AudioClip newClip, float transitionTime)
        {
            StartCoroutine(TransitionMusicCoroutine(newClip, transitionTime));
        }

        public void StartTransitionToNewMusic(string name, float transitionTime)
        {
            Sound sound = FindSound(name, musicSounds);

            StartCoroutine(TransitionMusicCoroutine(sound.clip, transitionTime));
        }

        private IEnumerator TransitionMusicCoroutine(AudioClip newClip, float transitionTime)
        {
            yield return StartCoroutine(FadeOutMusic(transitionTime));
            musicSource.clip = newClip;
            yield return StartCoroutine(FadeInMusic(transitionTime));
        }

        private IEnumerator FadeOutMusic(float time)
        {
            float startVolume = musicSource.volume;
            while (musicSource.volume > 0)
            {
                musicSource.volume -= startVolume * Time.deltaTime / time;
                yield return null;
            }

            musicSource.Stop();
            // musicSource.mute = true;
            musicSource.volume = startVolume;
        }

        private IEnumerator FadeInMusic(float time)
        {
            float targetVolume = musicSource.volume;
            musicSource.volume = 0;
            musicSource.mute = false;
            if (!musicSource.isPlaying)
                musicSource.Play();

            while (musicSource.volume < targetVolume)
            {
                musicSource.volume += targetVolume * Time.deltaTime / time;
                yield return null;
            }
        }

        private IEnumerator TransitionMandatorySfx(AudioClip newClip, float transitionTime)
        {
            float _time = transitionTime / 2;

            yield return StartCoroutine(FadeOutSourceVolume(sfxSource, _time));
            sfxSource.PlayOneShot(newClip);
            yield return StartCoroutine(FadeInSourceVolume(sfxSource, _time));
        }

        private IEnumerator FadeOutSourceVolume(AudioSource _source, float time)
        {
            float startVolume = _source.volume;

            while (_source.volume > 0)
            {
                _source.volume -= startVolume * Time.deltaTime / time;
                yield return null;
            }

            _source.Stop();

            _source.volume = startVolume;
        }

        private IEnumerator FadeInSourceVolume(AudioSource _source, float time)
        {
            float targetVolume = _source.volume;
            _source.volume = 0;
            _source.mute = false;
            if (!_source.isPlaying)
                _source.Play();

            while (_source.volume < targetVolume)
            {
                _source.volume += targetVolume * Time.deltaTime / time;
                yield return null;
            }
        }
    }
}