using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Seville
{
    public class OnHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private CarouselController _carouselController;
        private UnityEvent onClickEvent;
        private Image _imageButton;

        private Color _defaultColor;
        private Color _hoverColor;

        public void Initialize(CarouselController controller, UnityEvent onClick, Color defaultColor, Color hoverColor)
        {
            _imageButton = GetComponent<Image>();
            _carouselController = controller;

            onClickEvent = onClick;
            _defaultColor = defaultColor;
            _hoverColor = hoverColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClickEvent?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _imageButton.color = _hoverColor;
            _carouselController.OnHoverImage();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _imageButton.color = _defaultColor;
            _carouselController.OnClickImagge();
        }
    }
}
