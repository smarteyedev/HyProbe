using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Seville
{
    public class CarouselConfiguration : MonoBehaviour
    {
        [Header("Carousel Configuration")]
        public CarouselController _carouselController;
        public List<CarouselGaleryData> carouselData;
        public float fadeImageDuration;
        public bool showOnStart = false;

        [Header("Event Configurarion")]
        public UnityEvent onShowPanel;
        public UnityEvent onCompleteShowAllPanel;

        public bool setShowFirstTimeOnly = false;
        public bool setCompleteFirstTimeOnly = false;

        private void Start()
        {
            if (_carouselController != null)
            {
                _carouselController.Initialize(this, showOnStart);
            }
            else
            {
                Debug.LogError("Carousel Controller is Null");
            }
        }

        public void OnShowPanelCarousel()
        {
            if (onShowPanel != null)
            {
                onShowPanel.Invoke();

                if (setShowFirstTimeOnly)
                {
                    onShowPanel.RemoveAllListeners();
                }
            }
        }

        public void CheckFinishAllPanels()
        {
            foreach (CarouselGaleryData data in carouselData)
            {
                if (!data.hasShow)
                {
                    return;
                }
            }

            if (onCompleteShowAllPanel != null)
            {
                onCompleteShowAllPanel.Invoke();

                if (setCompleteFirstTimeOnly)
                {
                    onCompleteShowAllPanel.RemoveAllListeners();
                }
            }
        }
    }

    [System.Serializable]
    public class CarouselGaleryData
    {
        public Sprite imageSprite;
        [TextArea(3, 10)]
        public string descriptionImage;
        [HideInInspector] public bool hasShow = false;
    }
}
