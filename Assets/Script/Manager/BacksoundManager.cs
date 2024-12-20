using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public class BacksoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> _musicTracks;

        private int currentTrackIndex = 0;

        private void Start()
        {
            if (_musicTracks.Count > 0 && _audioSource != null)
            {
                PlayTrack(currentTrackIndex);
                StartCoroutine(CheckMusicStatus()); 
            }
            else
            {
                Debug.LogWarning("Pastikan AudioSource dan List _musicTracks telah diatur!");
            }
        }

        public void PlayTrack(int index)
        {
            if (_musicTracks.Count == 0) return;

            currentTrackIndex = index % _musicTracks.Count;
            _audioSource.clip = _musicTracks[currentTrackIndex];
            _audioSource.Play();
        }

        public void NextTrack()
        {
            if (_musicTracks.Count == 0) return;

            currentTrackIndex = (currentTrackIndex + 1) % _musicTracks.Count;
            PlayTrack(currentTrackIndex);
        }

        IEnumerator CheckMusicStatus()
        {
            while (true)
            {
                if (_audioSource.clip != null &&
                    _audioSource.time >= _audioSource.clip.length - 0.1f && // Toleransi 0.1 detik
                    _musicTracks.Count > 0)
                {
                    NextTrack();
                }
                yield return new WaitForSeconds(1f); // Cek setiap 1 detik
            }
        }
    }
}

