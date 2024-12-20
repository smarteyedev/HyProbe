using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Tproject.AudioManager;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using TMPro;

namespace Seville
{
    public class VideoPlayerController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private VideoData videoDataTemp;

        [Header("settings")]
        private float hideScreenControlTime = .9f;

        [Header("Component Dependencies")]
        [SerializeField] private VideoConfiguration videoConfig;
        [SerializeField] private VideoPlayer videoplayer;
        [SerializeField] private GameObject panelMain;
        [SerializeField] private TextMeshProUGUI textTitleVideo;
        [SerializeField] private GameObject panelThumbnail;
        [SerializeField] private GameObject buttonOpenPanelMain;
        [SerializeField] private GameObject buttonClosePanelMain;

        [Space(3f)]
        [SerializeField] private CanvasGroup controllerGroup;
        [SerializeField] private Slider sliderProgress;
        [SerializeField] private TextMeshProUGUI textDuration;
        [SerializeField] private GameObject activeControllerGroup;
        [SerializeField] private GameObject buttonPlay;
        [SerializeField] private GameObject buttonPause;
        [SerializeField] private GameObject buttonReverse;
        [SerializeField] private GameObject buttonForward;
        [SerializeField] private GameObject buttonReplay;

        [Header("UI Assets")]
        [SerializeField] private Sprite defaultThumbnailSprite;

        private AudioManager m_audioManager;
        private bool isPanelMainReady = false;
        private static List<VideoPlayerController> controllers = new List<VideoPlayerController>();

        private void Awake() =>
            controllers.Add(this);

        private void OnDestroy() =>
            controllers.Remove(this);

        private void Start()
        {
            videoplayer.loopPointReached += CheckerOnVideoEnd;

            if (AudioManager.Instance != null) m_audioManager = AudioManager.Instance;
            else Debug.LogWarning("Seville Videoplayer Controller: please add Audio Manager for the audio video output");

            StartCoroutine(GetAudioSourceCoroutine(() =>
            {
                if (videoConfig.playOnStart)
                {
                    OpenVideoplayerPanel(videoConfig.playOnStart);
                };
            }));

            SetupButtonFuction();

            if (sliderProgress)
                sliderProgress.onValueChanged.AddListener(HandleSliderChange);
        }

        private void Update()
        {
            if (panelMain.activeSelf)
            {
                UpdateRemainingTime();

                if (videoplayer.frameCount > 0 && sliderProgress)
                {
                    sliderProgress.SetValueWithoutNotify((float)videoplayer.frame / (float)videoplayer.frameCount);
                }
            }
        }

        public void OpenVideoplayerPanel(bool isAutoPlay = false)
        {
            if (videoDataTemp == null)
            {
                if (videoConfig.GetVideoData().videoClip == null)
                {
                    Debug.LogWarning($"Seville Videoplayer Controller: video clip is null");
                    return;
                }

                videoDataTemp = videoConfig.GetVideoData();
                videoplayer.clip = videoDataTemp.videoClip;
                textTitleVideo.text = string.IsNullOrEmpty(videoDataTemp.videoTitleText) ? "Videoplayer" : videoDataTemp.videoTitleText;
                panelThumbnail.GetComponent<Image>().sprite = videoDataTemp.thumbnailSprite != null ? videoDataTemp.thumbnailSprite : defaultThumbnailSprite;

                if (videoConfig.GetVideoData().thumbnailSprite == null)
                {
                    Debug.LogWarning($"Seville Videoplayer Controller: thumbnail video is null");
                }
            }

            UIAnimator.ScaleInObject(
                panelMain,
                () =>
                {
                    isPanelMainReady = false;
                    buttonOpenPanelMain.SetActive(false);
                },
                () =>
                {
                    SetVideoPlayState(isAutoPlay);
                    isPanelMainReady = true;
                }
            );
        }

        public void CloseVideoplayerPanel()
        {
            UIAnimator.ScaleOutObject(
                panelMain,
                () =>
                {
                    isPanelMainReady = false;
                    SetVideoPlayState(false);
                },
                () =>
                {
                    buttonOpenPanelMain.SetActive(true);
                    panelMain.SetActive(false);
                }
            );
        }

        #region Progress-bar
        void UpdateRemainingTime()
        {
            if (videoplayer != null && videoplayer.isPlaying)
            {
                double totalDuration = videoplayer.length;
                double currentTime = videoplayer.time;

                double remainingTime = totalDuration - currentTime;

                int minutes = Mathf.FloorToInt((float)remainingTime / 60F);
                int seconds = Mathf.FloorToInt((float)remainingTime % 60F);

                textDuration.text = $"{minutes:00}:{seconds:00}";
            }
        }

        private void HandleSliderChange(float value)
        {
            SkipToPercent(value);
        }

        private void SkipToPercent(float pct)
        {
            var frame = videoplayer.frameCount * pct;
            videoplayer.frame = (long)frame;
        }

        #endregion Progress-bar

        private IEnumerator GetAudioSourceCoroutine(Action OnGetSource)
        {
            while (AudioManager.Instance == null || AudioManager.Instance.videoSource == null)
            {
                Debug.Log("Seville Videoplayer Controller: Waiting for AudioSource to be available...");
                yield return null;
            }

            AudioSource videoAudioSource = AudioManager.Instance.videoSource;
            videoplayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoplayer.SetTargetAudioSource(0, videoAudioSource);
            // Debug.Log("Seville Videoplayer Controller: AudioSource successfully assigned to VideoPlayer.");

            OnGetSource.Invoke();
        }

        private void SetupButtonFuction()
        {
            buttonOpenPanelMain.GetComponent<Button>().onClick.AddListener(() => OpenVideoplayerPanel(false));
            buttonClosePanelMain.GetComponent<Button>().onClick.AddListener(() => CloseVideoplayerPanel());

            buttonPlay.GetComponent<Button>().onClick.AddListener(() => TogglePlayPause());
            buttonPause.GetComponent<Button>().onClick.AddListener(() => TogglePlayPause());
            buttonReverse.GetComponent<Button>().onClick.AddListener(() => OnClickReverseTime());
            buttonForward.GetComponent<Button>().onClick.AddListener(() => OnClickForwardTime());
            buttonReplay.GetComponent<Button>().onClick.AddListener(() => OnClickReplay());
        }

        private void PlayPauseVisibility()
        {
            buttonPause.SetActive(videoplayer.isPlaying);
            buttonPlay.SetActive(!videoplayer.isPlaying);
        }

        private void SetVideoPlayState(bool isPlaying)
        {
            foreach (var controller in controllers)
            {
                if (controller != this && controller.videoplayer.isPlaying)
                {
                    controller.videoplayer.Pause();
                    controller.UpdateUI(false);
                }
            }

            if (isPlaying)
            {
                videoplayer.Play();
                m_audioManager.musicSource.mute = true;
            }
            else
            {
                videoplayer.Pause();
                m_audioManager.musicSource.mute = false;
            }

            UpdateUI(isPlaying);
        }

        private void UpdateUI(bool isPlaying)
        {
            buttonPause.SetActive(isPlaying);
            buttonPlay.SetActive(!isPlaying);
            panelThumbnail.SetActive(!isPlaying);
        }

        private void CheckerOnVideoEnd(VideoPlayer vp)
        {
            if (!videoDataTemp.isWatched)
            {
                Debug.Log($"player has watched the video for the first time");
                videoConfig.UnityEvents.OnFisrtVideoWatched?.Invoke();
                videoDataTemp.isWatched = true;
            }

            videoConfig.UnityEvents.OnVideoWatched?.Invoke();

            panelThumbnail.SetActive(true);
            buttonReplay.SetActive(true);
            activeControllerGroup.SetActive(false);
        }

        #region Button-Function
        private void TogglePlayPause()
        {
            SetVideoPlayState(!videoplayer.isPlaying);
        }

        private void OnClickForwardTime() =>
            videoplayer.frame += 450;

        private void OnClickReverseTime() =>
            videoplayer.frame -= 450;

        private void OnClickReplay()
        {
            if (videoplayer != null)
            {
                videoplayer.Stop();
                videoplayer.time = 0;
                videoplayer.Play();

                activeControllerGroup.SetActive(true);
                UpdateUI(true);

                buttonReplay.SetActive(false);
            }
        }

        #endregion Button-Function

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetControllerVisibilty(true);
            PlayPauseVisibility();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (videoplayer.isPlaying)
                SetControllerVisibilty(false);
        }

        private void SetControllerVisibilty(bool visibilityState)
        {
            if (!isPanelMainReady) return;
            if (LeanTween.isTweening(controllerGroup.gameObject))
            {
                LeanTween.cancel(controllerGroup.gameObject);
            }

            controllerGroup.interactable = visibilityState;
            controllerGroup.blocksRaycasts = visibilityState;

            if (visibilityState)
            {
                LeanTween.alphaCanvas(controllerGroup, 1, 0.1f)
                                        .setEase(LeanTweenType.easeInOutQuad);
            }
            else
            {
                LeanTween.alphaCanvas(controllerGroup, 0, hideScreenControlTime)
                                        .setEase(LeanTweenType.easeInOutQuad);
            }
        }

        public void ResetVideoplayerController()
        {
            videoDataTemp = null;
            CloseVideoplayerPanel();

            buttonReplay.SetActive(false);
            activeControllerGroup.SetActive(true);
        }
    }
}
