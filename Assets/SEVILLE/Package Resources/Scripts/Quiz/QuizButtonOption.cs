using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Seville
{
    public class QuizButtonOption : MonoBehaviour
    {
#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField] private int indexBtn;
        public int GetAnswerIndex { get => indexBtn; }
#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField] private bool validate;
        public bool GetAnswerValidation { get => validate; }

        [Header("Component Dependencies")]
        [SerializeField] private Button btnOption;
        [SerializeField] private TextMeshProUGUI textOption;
        public string GetTextOption { get => textOption.text; }
        [SerializeField] private Image btnImage;

        [Header("Asset UI")]
        [SerializeField] private Sprite spriteDefault;
        [SerializeField] private Sprite spriteOnSelect;

        public void SetUpButton(int _index, bool _validate, string _text, bool _stateView, Action<int> _buttonAction)
        {
            indexBtn = _index;
            validate = _validate;
            textOption.text = _text;
            UpdateSelectedView(_stateView);
            btnOption.onClick.AddListener(() => _buttonAction(indexBtn));
        }

        public void UpdateSelectedView(bool isSelected)
        {
            if (isSelected) btnImage.sprite = spriteOnSelect;
            else btnImage.sprite = spriteDefault;
        }
    }
}