using DG.Tweening;
using Seville;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SR
{
    public class QuestionController : MonoBehaviour
    {
        [SerializeField] private ABaseNPCController _activeNPC;
        [SerializeField] private HistoryController _historyController;
        public TextMeshProUGUI dialogueText;
        [SerializeField] private List<ButtonChoiseComponent> _defaultButtons;
        [SerializeField] private List<ButtonChoiseComponent> _choiceButtons;
        
        [Header("Typing Properties")]
        [SerializeField] private float _typingSpeed = 20f;
        [SerializeField] private float _delayNext = 2f;
        [SerializeField] private float _delayAnswer = 3f;

        [Header("Question Settings")]
        [SerializeField] private List<QuestionData> _questionData;
        [SerializeField] private int _maxAttempt;
        [SerializeField] private int _currentAttemp;

        private QuestionData _currentQuestion;
        private int currentTextIndex = 0;
        private Tween _tweenTextTyping;
        private Coroutine _nextTyping = null;
        [SerializeField] private GameObject _panelQuestion;

        public void InitQuestion(List<QuestionData> questData, int maxAttempt, ABaseNPCController npc)
        {
            InitializeButtons();
            _questionData = new List<QuestionData>();

            if(_questionData != null)
            {
                _questionData.Clear();
            }

            _activeNPC = npc;
            _questionData = questData;
            _maxAttempt = maxAttempt;
            _currentAttemp = 0;
            GameplayManager.instance.OnAttemptQuestion(_currentAttemp);
        }

        public void ShowQuestion()
        {
            _historyController.ResetHistory();
            UIGameplayManager.instance.ShowChoiceDialogPanel(false);
            UIGameplayManager.instance.ShowChoiceQuestionPanel(true);
            ShowChoices();
        }

        private void InitializeButtons()
        {
            foreach (var button in _choiceButtons)
            {
                button._hasInteract = false;
            }
        }

        private void DisplayDialogue()
        {
            if (currentTextIndex < _currentQuestion.feedbackText.Count)
            {
                string currentText = _currentQuestion.feedbackText[currentTextIndex];
                Debug.Log(currentText);
                dialogueText.text = "";

                float typingDuration = currentText.Length / _typingSpeed;

                string text = "";
                _tweenTextTyping = DOTween.To(() => text, x => text = x, currentText, typingDuration).OnUpdate(() =>
                {
                    dialogueText.text = text;
                }
                ).OnComplete(() =>
                {
                    _nextTyping = StartCoroutine(IEOnTextDisplayed());
                });

                currentTextIndex++;
            }
            else
            {
                ShowChoices();
            }
        }

        private IEnumerator IEOnTextDisplayed()
        {
            if (currentTextIndex < _currentQuestion.feedbackText.Count)
            {
                yield return new WaitForSeconds(_delayNext);

                if (_nextTyping != null)
                {
                    DisplayDialogue();
                }
            }
            else
            {
                ShowPanelQuestion(true);
                ShowChoices();
            }

            if (_nextTyping != null)
            {
                StopCoroutine(_nextTyping);
            }
        }

        private void ShowChoices()
        {
            currentTextIndex = 0;
            for (int i = 0; i < _choiceButtons.Count; i++)
            {
                if (_choiceButtons[i] == null)
                    return;

                if (i < _questionData.Count)
                {
                    if (_choiceButtons[i]._hasInteract == false)
                    {
                        _choiceButtons[i].button.gameObject.SetActive(true);
                        _choiceButtons[i].textChoice.text = _questionData[i].questionText;
                    }
                }
                else
                {
                    _choiceButtons[i].button.gameObject.SetActive(false);
                }
            }
        }

        private void ClearAllChoices()
        {
            UIGameplayManager.instance.ShowNotification("Max Attempt, Silahkan Berikan Solusi");

            StartCoroutine(IEShowAnswer());
        }

        private IEnumerator IEShowAnswer()
        {
            yield return new WaitForSeconds(_delayAnswer);

            _activeNPC.ShowAnswer();
        }

        public void MakeChoice(int choiceIndex)
        {
            _currentQuestion = _questionData[choiceIndex];
            _choiceButtons[choiceIndex].button.gameObject.SetActive(false);
            _choiceButtons[choiceIndex]._hasInteract = true;
            _currentAttemp++;
            GameplayManager.instance.OnAttemptQuestion(_currentAttemp);

            _historyController.AddHistoryData(_currentQuestion.questionText, _currentQuestion.GetCombinedFeedbackText(), GetEnumFeedbackType(_currentQuestion.moodEffect));

            if (_currentQuestion.moodEffect != 0)
            {
                UIGameplayManager.instance.SetMoodValue(_activeNPC.UpdateMoodValue(_currentQuestion.moodEffect));
            }

            UIGameplayManager.instance.ShowReaction(_currentQuestion.moodEffect);
            _activeNPC.PlayAnimationTalking(_currentQuestion.moodEffect);


            if (_currentAttemp >= _maxAttempt)
            {
                ClearAllChoices();
            }
            else
            {
                DisplayDialogue();
            }

            ShowPanelQuestion(false);
        }

        private void ShowPanelQuestion(bool condition)
        {
            _panelQuestion.SetActive(condition);
            //if (condition)
            //{
            //    UIAnimator.ScaleInObject(_panelQuestion);
            //}
            //else
            //{
            //    UIAnimator.ScaleOutObject(_panelQuestion);
            //}
        }

        public EnumAnswerIconFeedback GetEnumFeedbackType(float value)
        {
            if(value > 0)
            {
                return EnumAnswerIconFeedback.Happy;
            }else if(value == 0)
            {
                return EnumAnswerIconFeedback.Netral;
            }
            else
            {
                return EnumAnswerIconFeedback.Sad;
            }
        }
    }
}

[System.Serializable]
public class QuestionData
{
    [TextArea(1,3)]
    public string questionText;
    [TextArea(1, 3)]
    public List<string> feedbackText;

    public float moodEffect;
    public UnityEvent onQuestionEvent;

    public bool isRequiedMood = false;
    public float minimumMood = 0;

    public string GetCombinedFeedbackText()
    {
        return string.Join("\n", feedbackText);
    }
}

[System.Serializable]
public class ButtonQuestionChoiceComponent
{
    public GameObject panel;
    public Button button;
    public TextMeshProUGUI textChoice;
}

public enum QuestionType
{
    Normal,
    MoodEffect
}
