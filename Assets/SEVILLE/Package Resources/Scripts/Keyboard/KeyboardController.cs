using UnityEngine.Events;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

namespace Seville
{
    [System.Serializable]
    public class StringKeyboardOutput : UnityEvent<string> { }

    public class KeyboardController : MonoBehaviour
    {
        public NonNativeKeyboard keyboard;
        public TMP_InputField inputField;

        public float distance = 0.5f;
        public float verticalOffset = -0.5f;

        public Transform possitionCam;

        [Space]
        public StringKeyboardOutput OnGetKeyboardOuput;

        void Start()
        {
            if (!keyboard) Debug.LogWarning("NonNativeKeyboard hasn't been assigned");

            if (OnGetKeyboardOuput == null)
                OnGetKeyboardOuput = new StringKeyboardOutput();

            inputField.onSelect.AddListener(x => OpenKeyboard());
        }

        public void OpenKeyboard()
        {
            if (keyboard.gameObject.activeSelf == false) keyboard.gameObject.SetActive(true);

            keyboard.InputField = inputField;
            keyboard.PresentKeyboard(inputField.text);

            if (possitionCam != null)
            {
                Vector3 direction = possitionCam.forward;
                direction.y = 0;
                direction.Normalize();

                Vector3 targetPos = possitionCam.position + direction * distance + Vector3.up * verticalOffset;
                keyboard.RepositionKeyboard(targetPos);

                Debug.LogWarning($"The keyboard controller does not find the moodValue of positionCam, enter the transform camera origin into a variable");
            }

            SetCaretColorAlpha(1);

            keyboard.OnClosed += Instance_OnClosed;
        }

        private void Instance_OnClosed(object sender, System.EventArgs a)
        {
            SetCaretColorAlpha(0);
            keyboard.OnClosed -= Instance_OnClosed;
        }

        public void SetCaretColorAlpha(float value)
        {
            inputField.customCaretColor = true;
            Color caretColor = inputField.caretColor;
            caretColor.a = value;
            inputField.caretColor = caretColor;
        }

        public void OnClickSubmit()
        {
            string msg = inputField.text.ToString();

            // Debug.Log($"player submit text: '{msg}'");

            if (OnGetKeyboardOuput != null)
                OnGetKeyboardOuput.Invoke(msg);
        }
    }
}