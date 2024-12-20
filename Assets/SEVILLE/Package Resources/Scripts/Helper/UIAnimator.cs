using System;
using UnityEngine;

namespace Seville
{
    public static class UIAnimator
    {
        public static void ScaleInObject(GameObject targetObject, Action onStart = null, Action onComplete = null)
        {
            if (targetObject == null) return;

            if (LeanTween.isTweening(targetObject))
            {
                LeanTween.cancel(targetObject);
            }

            if (!targetObject.activeSelf)
            {
                targetObject.SetActive(true);
                targetObject.transform.localScale = Vector3.zero;
            }

            // Debug.Log("Seville UI Animator: start scale in object");
            onStart?.Invoke();

            LeanTween.scale(targetObject, Vector3.one, 1f)
                .setEase(LeanTweenType.easeOutQuint)
                .setOnComplete(() =>
                {
                    // Debug.Log("Seville UI Animator: finish scale in object");
                    onComplete?.Invoke();
                });
        }

        public static void ScaleOutObject(GameObject targetObject, Action onStart = null, Action onComplete = null)
        {
            if (targetObject == null || !targetObject.activeSelf) return;

            if (LeanTween.isTweening(targetObject))
            {
                LeanTween.cancel(targetObject);
            }

            // Debug.Log("Seville UI Animator: start scale out object");
            onStart?.Invoke();

            LeanTween.scale(targetObject, Vector3.zero, 0.5f)
                .setEase(LeanTweenType.easeInQuad)
                .setOnComplete(() =>
                {
                    // Debug.Log("Seville UI Animator: finish scale out object");
                    targetObject.SetActive(false);
                    onComplete?.Invoke();
                });
        }

        public static void FadeOutFadeInCanvasGroup(CanvasGroup targetObject, Action onStart = null, Action onComplete = null)
        {
            if (targetObject == null) return;

            // Debug.Log("Seville UI Animator: start fade out-fade in canvas group");
            onStart?.Invoke();

            if (LeanTween.isTweening(targetObject.gameObject))
            {
                LeanTween.cancel(targetObject.gameObject);
            }

            LeanTween.alphaCanvas(targetObject, 0f, 0.5f)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    onComplete?.Invoke();

                    LeanTween.alphaCanvas(targetObject, 1f, 1f)
                        .setEase(LeanTweenType.easeInOutQuad)
                        .setOnComplete(() =>
                        {
                            // Debug.Log("Seville UI Animator: finish fade in canvas group");
                        });
                });
        }
    }
}
