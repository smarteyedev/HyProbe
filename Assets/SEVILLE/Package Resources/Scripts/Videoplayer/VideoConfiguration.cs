using UnityEngine;
using UnityEngine.Video;
using System;
using UnityEngine.Events;

namespace Seville
{
    public class VideoConfiguration : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private VideoData videoData;

        [Header("Settings")]
        public bool playOnStart = false;

        [Header("Quest Event")]
        public VideoEvent UnityEvents;

        public VideoData GetVideoData()
        {
            return new VideoData(videoData);
        }
    }

    [Serializable]
    public class VideoEvent
    {
        [Space(4f)]
        [Tooltip("The OnFisrtVideoWatched event on firstime video watched.")]
        public UnityEvent OnFisrtVideoWatched;

        [Tooltip("The OnVideoFinished event will be called after the video end.")]
        public UnityEvent OnVideoWatched;
    }

    [Serializable]
    public class VideoData
    {
        public string videoTitleText;
        public VideoClip videoClip;
        public Sprite thumbnailSprite;

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public bool isWatched = false;

        public VideoData(VideoData other)
        {
            videoTitleText = other.videoTitleText;
            videoClip = other.videoClip;
            thumbnailSprite = other.thumbnailSprite;
            isWatched = other.isWatched;
        }
    }
}