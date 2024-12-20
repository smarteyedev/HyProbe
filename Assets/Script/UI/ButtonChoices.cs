using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SR
{
    public class ButtonChoices : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.instance.PlayClickButton();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.instance.PlayHoverButton();
        }
    }
}
