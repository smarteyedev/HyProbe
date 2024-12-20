using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Seville
{
    public class ActivateGrabRay : MonoBehaviour
    {
        public GameObject rightGrabRay;
        public GameObject leftGrabRay;

        public XRDirectInteractor rightDirectGrab;
        public XRDirectInteractor leftDirectGrab;

        private void Update()
        {
            leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
            rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);
        }
    }
}