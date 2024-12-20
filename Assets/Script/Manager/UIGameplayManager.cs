using DG.Tweening;
using Seville;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SR
{
    public class UIGameplayManager : MonoBehaviour
    {
        public static UIGameplayManager instance;

        [Header("Dialog Component")]
        [SerializeField] private TextMeshProUGUI _dialogText;
        [SerializeField] private float _typingSpeed = 50f;
        private Tween _tweenTextTyping;

        [Header("NPC Characteristic")]
        [SerializeField] private GameObject panelCharacteristic;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textDesc;
        [SerializeField] private Slider trustSlider;
        [SerializeField] private Slider socialSlider;
        [SerializeField] private Slider intelligenceSlider;
        [SerializeField] private Slider funnySlider;

        [Header("Panel Component")]
        [SerializeField] private GameObject _nextPanel;
        [SerializeField] private GameObject _selectorPanel;
        [SerializeField] private GameObject _dialogPanel;
        [SerializeField] private GameObject _choicesPanel;
        [SerializeField] private GameObject _choiceQuestionPanel;
        [SerializeField] private GameObject _answerPanel;

        [SerializeField] private TextMeshProUGUI _dialogNpcName;

        [Header("Panel Mood")]
        [SerializeField] private GameObject _panelSlider;
        [SerializeField] private Slider _sliderMood;
        [SerializeField] private Image _imageSlider;
        [SerializeField] private Image _imageMoodIcon;
        [SerializeField] private List<Sprite> _sliderSprite;
        [SerializeField] private List<Sprite> _iconSprite;
        [SerializeField] private float _sliderDuration = 1f;


        [Header("Notification Panel")]
        [SerializeField] private GameObject _panelNotification;
        [SerializeField] private Animator _animatorNotification;
        [SerializeField] private TextMeshProUGUI _textNotification;
        [SerializeField] private float _durationNotification = 5f;
        private Coroutine _notificationCoroutine = null;

        [Header("Result")]
        [SerializeField] private TextMeshProUGUI _resultNPCName;

        [Header("Reaction Component")]
        [SerializeField] private GameObject _panelReaction;
        [SerializeField] private Image _reactionImage;
        [SerializeField] private Sprite _happyReaction;
        [SerializeField] private Sprite _netralReaction;
        [SerializeField] private Sprite _sadReaction;
        [SerializeField] private float _delayReaction = 4f;

        private void Awake()
        {
            instance = this;
        }

        public void ShowSelectorPanel(bool condition)
        {
            _selectorPanel.SetActive(condition);
        }

        public void ShowDialogPanel(bool condition)
        {
            _choicesPanel.SetActive(true);
            AnimatePanel(condition, _dialogPanel);
            AnimatePanel(condition, _choicesPanel);
        }

        public void ShowChoiceDialogPanel(bool condition)
        {
            _choicesPanel.SetActive(false);
            AnimatePanel(condition, _choicesPanel);
        }

        public void ShowChoiceQuestionPanel(bool condition)
        {
            _choiceQuestionPanel.SetActive(condition);
            //AnimatePanel(condition, );
        }


        #region Slider Mood Region

        public void ShowSliderMood(bool condition)
        {
            AnimatePanel(condition, _panelSlider);
        }

        public void AnimatePanel(bool condition, GameObject obj)
        {
            if (condition)
            {
                UIAnimator.ScaleInObject(obj);
            }
            else
            {
                UIAnimator.ScaleOutObject(obj);
            }
        }

        public void SetMoodValue(float value)
        {
            StartCoroutine(IEAnimateSliderCoroutine(value));
        }

        private IEnumerator IEAnimateSliderCoroutine(float targetValue)
        {
            float startValue = _sliderMood.value; 
            float elapsedTime = 0f;

            while (elapsedTime < _sliderDuration)
            {
                elapsedTime += Time.deltaTime;
                float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / _sliderDuration);
                _sliderMood.value = newValue; 
                yield return null; 
            }

            _sliderMood.value = targetValue;

            if(_sliderMood.value < 0f)
            {
                _sliderMood.value = 0;
            }

            UpdateSpriteSliderMood(_sliderMood.value);
        }

        private void UpdateSpriteSliderMood(float value)
        {
            if(value >= 70f)
            {
                _imageSlider.sprite = _sliderSprite[0];
                _imageMoodIcon.sprite = _iconSprite[0];
            }
            else if (value < 70 && value >= 40)
            {
                _imageSlider.sprite = _sliderSprite[1];
                _imageMoodIcon.sprite = _iconSprite[1];
            }
            else if(value < 40)
            {
                _imageSlider.sprite = _sliderSprite[2];
                _imageMoodIcon.sprite = _iconSprite[2];
            }
            else
            {
                _imageSlider.sprite = _sliderSprite[1];
                _imageMoodIcon.sprite = _iconSprite[1];
            }
        }

        #endregion

        public void SetNPCTextName(string name)
        {
            Debug.Log(name);
            _dialogNpcName.text = name;
            _resultNPCName.text = name;
        }

        public void DisplayDialogue(string textDialog, UnityAction callback)
        {
            _dialogText.text = "";

            float typingDuration = textDialog.Length / _typingSpeed;

            string text = "";
            _tweenTextTyping = DOTween.To(() => text, x => text = x, textDialog, typingDuration).OnUpdate(() =>
            {
                _dialogText.text = text;
            }
            ).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        private void OnUpdateNPCStatus(NPCStatus status)
        {
            panelCharacteristic.SetActive(true);
            //textName.text = status.npcName;
            //textDesc.text = status.npcDescription;
            //trustSlider.moodValue = status.trust;
            //socialSlider.moodValue = status.social;
            //intelligenceSlider.moodValue = status.intelligence;
            //funnySlider.moodValue = status.funny;
        }

        public void ShowNotification(string message)
        {
            if (_notificationCoroutine != null)
                return;

            AudioManager.instance.PlayPopupButon();
            _textNotification.text = message;
            _notificationCoroutine = StartCoroutine(IEShowNotification());
        }

        private IEnumerator IEShowNotification()
        {
            _animatorNotification.SetBool("IsShow", true);

            yield return new WaitForSeconds(_durationNotification);

            _animatorNotification.SetBool("IsShow", false);
            _notificationCoroutine = null;
        }

        public void ShowPanelAnswer(bool condition)
        {
            _answerPanel.SetActive(condition);
        }

        public void ShowNextPanel(bool condition)
        {
            _nextPanel.SetActive(condition);
        }


        #region Reaction Region

        public void ShowReaction(float value)
        {
            if (value > 0)
            {
                _reactionImage.sprite = _happyReaction;
            }else if(value == 0)
            {
                _reactionImage.sprite = _netralReaction;
            }
            else
            {
                _reactionImage.sprite = _sadReaction;
            }
            AnimatePanel(true, _panelReaction);

            StartCoroutine(IEDelayUnshowReaction());

        }

        private IEnumerator IEDelayUnshowReaction()
        {
            yield return new WaitForSeconds(_delayReaction);

            AnimatePanel(false, _panelReaction);
        }

        #endregion
    }
}
