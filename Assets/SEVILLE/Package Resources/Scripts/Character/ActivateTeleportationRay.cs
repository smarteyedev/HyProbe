using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

namespace Seville
{
    public class ActivateTeleportationRay : MonoBehaviour
    {
        public GameObject rightTeleportation;
        public GameObject leftTeleportation;

        public InputActionProperty rightActive;
        public InputActionProperty leftActive;

        public InputActionProperty rightCancle;
        public InputActionProperty leftCancle;

        public XRRayInteractor rightRay;
        public XRRayInteractor leftRay;

        private void Update()
        {
            bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPost, out Vector3 leftNormal, out int leftNumber, out bool leftValid);

            leftTeleportation.SetActive(!isLeftRayHovering && leftCancle.action.ReadValue<float>() == 0 && leftActive.action.ReadValue<float>() > 0.1f);

            bool isRightRayHovering = rightRay.TryGetHitInfo(out Vector3 rightPost, out Vector3 rightNormal, out int rightNumber, out bool rightValid);

            rightTeleportation.SetActive(!isRightRayHovering && rightCancle.action.ReadValue<float>() == 0 && rightActive.action.ReadValue<float>() > 0.1f);
        }
    }
}