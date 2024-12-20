using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Seville
{
    public class CanvasRayChecker : TrackedDeviceGraphicRaycaster
    {

        [Header("SmartEye Framework")]
        public bool isTrigger = false;
        [HideInInspector] public bool isPlayerHoverCanvas;

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            base.Raycast(eventData, resultAppendList);

            CheckerRayCast(eventData);
        }

        private void CheckerRayCast(PointerEventData eventData)
        {
            if (!isTrigger) return;

            if (eventData.pointerEnter && eventData.hovered.Find((x) => x.gameObject.CompareTag("VideoPlayerCanvas")))
            {
                isPlayerHoverCanvas = true;
            }

            else isPlayerHoverCanvas = false;
        }
    }
}