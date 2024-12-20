using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seville
{
    public class AnimatorHandController : MonoBehaviour
    {
        public InputActionProperty pitchAnimationAction;
        public InputActionProperty gripAnimationAction;
        public Animator handAnimator;

        private void Update()
        {
            float pitchValue = pitchAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", pitchValue);

            float gripValue = gripAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);
        }
    }
}