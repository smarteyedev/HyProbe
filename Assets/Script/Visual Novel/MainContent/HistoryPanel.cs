using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SR
{
    public class HistoryPanel : MonoBehaviour
    {
        [Header("UI Compoenent")]
        [SerializeField] private TextMeshProUGUI _textQuestion;
        [SerializeField] private TextMeshProUGUI _textAnswer;
        [SerializeField] private Image _iconSprite;
        [SerializeField] private Image _background;
        [SerializeField] private Image _bgAnswer;

        private UIComponentHistoryPanel _uiComponent;

        public void Initialize(UIComponentHistoryPanel component)
        {
            _uiComponent = component;
        }

        public void SetUIOPanel(string textQuestion, string textAnswer, EnumAnswerIconFeedback type)
        {
            gameObject.SetActive(true);
            _textQuestion.text = textQuestion;
            _textAnswer.text = textAnswer;

            SetUICompoent(type);
        }

        private void SetUICompoent(EnumAnswerIconFeedback type)
        {
            switch (type)
            {
                case EnumAnswerIconFeedback.Happy:
                    _iconSprite.sprite = _uiComponent.iconHappy;
                    _background.color = _uiComponent.backgroundColorHappy;
                    _bgAnswer.color = _uiComponent.bgAnswerColorHappy;
                    break;

                case EnumAnswerIconFeedback.Netral:
                    _iconSprite.sprite = _uiComponent.iconNetral;
                    _background.color = _uiComponent.backgroundColorNetral;
                    _bgAnswer.color = _uiComponent.bgAnswerColorNetral;
                    break;

                case EnumAnswerIconFeedback.Sad:
                    _iconSprite.sprite = _uiComponent.iconSad;
                    _background.color = _uiComponent.backgroundColorSad;
                    _bgAnswer.color = _uiComponent.bgAnswerColorSad;
                    break;

                default:

                    break;
            }
        }
    }
}
