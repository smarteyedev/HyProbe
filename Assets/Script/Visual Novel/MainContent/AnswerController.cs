using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SR
{
    public class AnswerController : MonoBehaviour
    {
        [SerializeField] private ResultController _resultController;
        private List<AnswerSolution> _solutions;
        [SerializeField] private List<ButtonChoiseComponent> _choiceButton;
        [SerializeField] private Slider _moodSlider;
        [SerializeField] private Sprite _npcSprite;

        private int _indexChoice = 0;

        [SerializeField] private float _delayShowResult = 3f;

        public void InitAnswer(List<AnswerSolution> solutions, string startingDialog, Sprite sprite)
        {
            _solutions = new List<AnswerSolution>();
            _npcSprite = sprite;

            _solutions = solutions;

            UIGameplayManager.instance.DisplayDialogue(startingDialog, ShowSolutions);
        }

        private void ShowSolutions()
        {
            UIGameplayManager.instance.ShowPanelAnswer(true);
            ResetAllButton();
            for (int i = 0; i < _solutions.Count; i++)
            {
                _choiceButton[i].textChoice.text = _solutions[i].answerText;

                _choiceButton[i].button.gameObject.SetActive(true);
            }
        }

        public void MakeChoice(int index)
        {
            if (_solutions[index] == null)
            {
                Debug.LogError("Solusi Tidak Ditemukan");
                return;
            }


            _indexChoice = index;

            if (_solutions[index].answerPoint >= 80)
            {
                
            }
            else if(_solutions[index].answerPoint >= 40 && _solutions[index].answerPoint < 80)
            {

            }else if(_solutions[index].answerPoint < 40)
            {

            }

            GameplayManager.instance.PlayTalkingNPC(_solutions[index].moodPoint);

            UIGameplayManager.instance.SetMoodValue(GameplayManager.instance._activeNPC.UpdateMoodValue(_solutions[index].moodPoint));
            UIGameplayManager.instance.ShowPanelAnswer(false);
            UIGameplayManager.instance.DisplayDialogue(_solutions[index].endText, OnTimeCompleted);
        }

        private void OnTimeCompleted()
        {
            StartCoroutine(ShowResult());
        }

        private IEnumerator ShowResult()
        {
            yield return new WaitForSeconds(_delayShowResult);

            GameplayManager.instance.PlayFadeoutNPC();
            UIGameplayManager.instance.ShowDialogPanel(false);
            UIGameplayManager.instance.ShowSliderMood(false);


            yield return new WaitForSeconds(_delayShowResult);

            ResultData result = new ResultData();

            result.textEnding = _solutions[_indexChoice].ending;
            result.resultPoint = _solutions[_indexChoice].answerPoint;
            result.moodValue = _moodSlider.value;
            result.npcSprite = _npcSprite;

            _resultController.SetUIResult(result);
        }

        private void ResetAllButton()
        {
            foreach (var button in _choiceButton)
            {
                button.button.gameObject.SetActive(false);
            }
        }
    }
}
