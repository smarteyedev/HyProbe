using TMPro;
using UnityEngine;

namespace Seville
{
    public class KeyboardToTMProUGUI : MonoBehaviour
    {
        public TextMeshProUGUI outputTargetText;

        public void EventStringReceiver(string message)
        {
            if (outputTargetText != null)
            {
                outputTargetText.text = message;
                // Debug.Log("Event received with message: " + message);
            }
            else
            {
                Debug.LogError($"outputTargetText is null reference, plase drag output target");
            }
        }
    }
}