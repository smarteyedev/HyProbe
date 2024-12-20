
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Seville
{
    public class IntegersCounters : MonoBehaviour
    {
#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField] private int currentValue;
        [SerializeField] private int valueMax;
        private bool isValueMaxReached = false;

        [Space]
        public UnityEvent OnMaximalValue;
        [Space]

        public TextMeshProUGUI debugValueText;

        void Start()
        {
            currentValue = 0;
            UpdateValueText();
        }

        public void IncreaseValue(int amount)
        {
            if (isValueMaxReached)
            {
                Debug.Log("Score max reached, cannot add more score.");
                return;
            }

            currentValue += amount;
            UpdateValueText();

            CheckScore();
        }

        public void DecreaseValue(int amount)
        {
            if (isValueMaxReached)
            {
                Debug.Log("Score max reached, cannot subtract score.");
                return;
            }

            currentValue -= amount;
            if (currentValue < 0)
            {
                currentValue = 0;
            }
            UpdateValueText();

            CheckScore();
        }

        private void CheckScore()
        {
            if (currentValue >= valueMax && !isValueMaxReached)
            {
                isValueMaxReached = true;
                OnMaximalValue.Invoke();
            }
        }

        public int GetCurrentValue()
        {
            return currentValue;
        }

        private void UpdateValueText()
        {
            if (debugValueText != null)
            {
                debugValueText.text = "Score: " + currentValue.ToString();
            }
        }
    }
}