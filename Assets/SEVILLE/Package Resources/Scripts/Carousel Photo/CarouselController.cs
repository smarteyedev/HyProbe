using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using Tproject.AudioManager;

namespace Seville
{
    public class CarouselController : MonoBehaviour
    {
        private CarouselConfiguration _carouselConfiguration;

        [Header("Panel Canvas")]
        [SerializeField] private GameObject _carouselPanel;
        [SerializeField] private GameObject _carouselShowButton;

        [Header("Carousel Controller")]
        public bool editCarouselController;
        [SerializeField] private Image _displayImage;
        [SerializeField] private TextMeshProUGUI _textDescription;
        [SerializeField] private Transform _dotContainer;
        [SerializeField] private GameObject _dotPrefab;
        [SerializeField] private CanvasGroup _bodyCanvasGroup;

        [Header("Button Controller")]
        public bool editButtonController;
        [SerializeField] private Image _buttonNext;
        [SerializeField] private Image _buttonPrevious;
        [SerializeField] private Image _closeButton;
        [SerializeField] private Color _hoverColor;
        [SerializeField] private Color _defaultColor;
        //[SerializeField] private string _hoverClipName;
        //[SerializeField] private string _pressedClipName;
        private int _currentIndex = 0;

        [Header("Dots Controller")]
        public bool editDotsController;
        [SerializeField] private Color _dotsDefaultColor = new Color(255, 255, 255, 0.2f);
        [SerializeField] private Color _dotsActiveColor = new Color(255, 255, 255, 1);

        private List<Image> dots = new List<Image>();
        public bool isTransitioning = false;

        public void Initialize(CarouselConfiguration config, bool showOnStart)
        {
            _carouselConfiguration = config;

            if (_carouselConfiguration.carouselData.Count > 0)
            {
                TransitionToImage(_currentIndex);
                CreateDots();
                UpdateDots();
            }

            InitializeEvents();

            if (showOnStart)
            {
                OpenPanel();
            }
            else
            {
                ClosePanel();
            }
        }

        private void InitializeEvents()
        {
            OnHoverButton nextButton = _buttonNext.gameObject.AddComponent<OnHoverButton>();
            UnityEvent nextClickEvent = new UnityEvent();
            nextClickEvent.AddListener(NextImage);
            nextButton.Initialize(this, nextClickEvent, _defaultColor, _hoverColor);

            OnHoverButton previousButton = _buttonPrevious.gameObject.AddComponent<OnHoverButton>();
            UnityEvent prevClickEvent = new UnityEvent();
            prevClickEvent.AddListener(PreviousImage);
            previousButton.Initialize(this, prevClickEvent, _defaultColor, _hoverColor);

            OnHoverButton closeButton = _closeButton.gameObject.AddComponent<OnHoverButton>();
            UnityEvent closeClickEvent = new UnityEvent();
            closeClickEvent.AddListener(ClosePanel);
            closeButton.Initialize(this, closeClickEvent, _defaultColor, _hoverColor);

            OnHoverButton showButton = _carouselShowButton.gameObject.AddComponent<OnHoverButton>();
            UnityEvent showClickEvent = new UnityEvent();
            showClickEvent.AddListener(OpenPanel);
            showButton.Initialize(this, showClickEvent, _defaultColor, _hoverColor);
        }

        public void OnHoverImage()
        {
            //AudioManager.Instance.PlaySFX(_hoverClipName);
        }

        public void OnClickImagge()
        {
            //AudioManager.Instance.PlaySFX(_pressedClipName);
        }

        public void OpenPanel()
        {
            if (_carouselConfiguration.carouselData.Count == 0)
            {
                if (_carouselPanel.activeSelf) ClosePanel();

                Debug.LogWarning($"Seville Carousel Controller: the carousel data list is null");
                return;
            }

            UIAnimator.ScaleOutObject(_carouselShowButton);
            UIAnimator.ScaleInObject(_carouselPanel);

            _carouselConfiguration.OnShowPanelCarousel();
        }

        public void ClosePanel()
        {
            UIAnimator.ScaleOutObject(_carouselPanel);
            UIAnimator.ScaleInObject(_carouselShowButton);
        }

        public void NextImage()
        {
            if (isTransitioning || _carouselConfiguration.carouselData.Count == 0) return;

            _buttonNext.raycastTarget = false;
            int nextIndex = (_currentIndex + 1) % _carouselConfiguration.carouselData.Count;
            TransitionToImage(nextIndex);
        }

        public void PreviousImage()
        {
            if (isTransitioning || _carouselConfiguration.carouselData.Count == 0) return;

            _buttonPrevious.raycastTarget = false;
            int prevIndex = (_currentIndex - 1 + _carouselConfiguration.carouselData.Count) % _carouselConfiguration.carouselData.Count;

            TransitionToImage(prevIndex);
        }

        private void TransitionToImage(int newIndex)
        {
            isTransitioning = true;

            _currentIndex = newIndex;
            _carouselConfiguration.carouselData[newIndex].hasShow = true;
            _carouselConfiguration.CheckFinishAllPanels();

            UIAnimator.FadeOutFadeInCanvasGroup(_bodyCanvasGroup, null, OnChangeBody);
        }

        private void OnChangeBody()
        {
            _displayImage.sprite = _carouselConfiguration.carouselData[_currentIndex].imageSprite;
            _textDescription.text = _carouselConfiguration.carouselData[_currentIndex].descriptionImage;
            isTransitioning = false;


            _buttonNext.raycastTarget = true;
            _buttonPrevious.raycastTarget = true;

            UpdateDots();
        }


        private void CreateDots()
        {
            GameObject TempDots = _dotPrefab;
            for (int i = 0; i < _carouselConfiguration.carouselData.Count; i++)
            {
                if (_carouselConfiguration.carouselData.Count > 1)
                {
                    GameObject dot = Instantiate(_dotPrefab, _dotContainer);
                    Image dotImage = dot.GetComponent<Image>();
                    dots.Add(dotImage);
                }
            }

            if (_carouselConfiguration.carouselData.Count > 1)
            {
                _dotPrefab = dots[1].gameObject;
                Destroy(TempDots);
            }

            UpdateDots();
        }

        private void UpdateDots()
        {
            for (int i = 0; i < dots.Count; i++)
            {
                if (i == _currentIndex)
                {
                    dots[i].color = _dotsActiveColor;
                }
                else
                {
                    dots[i].color = _dotsDefaultColor;
                }
            }
        }
    }
}
