using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Seville
{
    public class XROffsetGrabInteractable : XRGrabInteractable
    {
        private Vector3 initialLocalPos;
        private Quaternion initialLocalRot;

        private void Start()
        {
            if (!attachTransform)
            {
                GameObject attachPoint = new GameObject("Offset Grab Pivot");
                attachPoint.transform.SetParent(transform, false);
                attachTransform = attachPoint.transform;
            }
            else
            {
                initialLocalPos = attachTransform.localPosition;
                initialLocalRot = attachTransform.localRotation;
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (args.interactorObject is XRDirectInteractor)
            {
                attachTransform.localPosition = args.interactorObject.transform.position;
                attachTransform.localRotation = args.interactorObject.transform.rotation;
            }
            else
            {
                attachTransform.localPosition = initialLocalPos;
                attachTransform.localRotation = initialLocalRot;
            }


            base.OnSelectEntered(args);
        }
    }
}