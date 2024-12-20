using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Seville
{
    public class QuizReviewItem : MonoBehaviour
    {
        [Header("Component Dependencies")]
        [SerializeField] private Sprite correctAnswerSprite;
        [SerializeField] private Sprite wrongAnswerSprite;

        [Header("Component Dependencies")]
        [SerializeField] TextMeshProUGUI textNumber;
        [SerializeField] TextMeshProUGUI textQuestion;
        [SerializeField] TextMeshProUGUI textPlayerAnswer;
        [SerializeField] TextMeshProUGUI textCorrectAnswer;
        [SerializeField] Image imgBackground;

        [SerializeField] private RectTransform[] rectTransforms;

        public void SetUpReviewItem(bool _isCorrectAnswer, string _number, string _question, string _playerAnswer, string _correctAnswer)
        {
            imgBackground.sprite = _isCorrectAnswer ? correctAnswerSprite : wrongAnswerSprite;

            textNumber.text = _number;
            textQuestion.text = _question;
            textPlayerAnswer.text = _playerAnswer;
            textCorrectAnswer.text = _correctAnswer;
        }

        private void OnEnable()
        {
            StartCoroutine(RefreshLayout());
        }

        IEnumerator RefreshLayout()
        {
            foreach (var item in rectTransforms)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(item);
                yield return new WaitForEndOfFrame(); // wait for a frame
            }
        }
    }
}