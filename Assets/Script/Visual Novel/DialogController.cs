using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace SR
{
    public class DialogController : MonoBehaviour
    {
        private ABaseNPCController _activeNPC;
        [SerializeField] private DialogueGraph dialogueGraph;

        public TextMeshProUGUI dialogueText;
        [SerializeField] private QuestionController _questionController;
        public ButtonChoiseComponent[] choiceButtons;
        private DialogueNode currentNode;

        private List<GameObject> _activeButton = new List<GameObject>();

        public System.Action onDialogueEnd;
        private int currentTextIndex = 0;
        private Tween _tweenTextTyping;
        private Coroutine _nextTyping = null;

        [Header("Typing Properties")]
        [SerializeField] private float _typingSpeed = 10f;
        [SerializeField] private float _delayNext = 5f;

        public void ShowDialog(DialogGraphController activeDialog, List<QuestionData> questData, int maxAttempt, ABaseNPCController npc)
        {
            dialogueGraph = new DialogueGraph();

            _activeNPC = npc;
            currentTextIndex = 0;
            dialogueGraph = activeDialog.dialogData;
            currentNode = dialogueGraph.startingNode;
            _questionController.InitQuestion(questData, maxAttempt, _activeNPC);

            UIGameplayManager.instance.ShowSliderMood(true);
            UIGameplayManager.instance.SetMoodValue(_activeNPC.moodValue);
            DisplayDialogue();
        }

        private void DisplayDialogue()
        {
            _activeNPC.PlayAnimationTalking(0);

            if (currentTextIndex < currentNode.dialogueText.Count)
            {
                string currentText = currentNode.dialogueText[currentTextIndex];
                Debug.Log(currentText);
                dialogueText.text = ""; 

                float typingDuration = currentText.Length / _typingSpeed;

                string text = "";
                _tweenTextTyping = DOTween.To(() => text, x => text = x, currentText, typingDuration).OnUpdate(() =>
                {
                    dialogueText.text = text;
                    //Debug.Log("Update Text");
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
            if (currentTextIndex < currentNode.dialogueText.Count)
            {
                yield return new WaitForSeconds(_delayNext);

                if(_nextTyping != null)
                {
                    DisplayDialogue();
                }
            }
            else
            {
                ShowChoices();
            }

            if(_nextTyping != null)
            {
                StopCoroutine(_nextTyping);
            }

            if (currentNode.choices.Count == 0)
            {
                yield return new WaitForSeconds(_delayNext);
                EndDialogue();
            }
        }

        private void ShowChoices()
        {
            currentTextIndex = 0;
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i < currentNode.choices.Count)
                {
                    if (choiceButtons[i].button != null)
                    {
                        choiceButtons[i].button.gameObject.SetActive(true);
                        choiceButtons[i].textChoice.text = currentNode.choices[i].choiceText;
                    }

                    _activeButton.Add(choiceButtons[i].button.gameObject);
                }
                else
                {
                    choiceButtons[i].button.gameObject.SetActive(false);
                }
            }
        }

        public void MakeChoice(int choiceIndex)
        {
            currentNode = currentNode.choices[choiceIndex].nextNode;
            DisplayDialogue();
            HideActiveButton();
            _activeButton.Clear();
        }

        private void HideActiveButton()
        {
            foreach(GameObject obj in _activeButton)
            {
                obj.SetActive(false);
            }
        }

        private void EndDialogue()
        {
            Debug.Log("Dialogue ended.");
            onDialogueEnd?.Invoke();
            _questionController.ShowQuestion();
        }
    }
}

[System.Serializable]
public class ButtonChoiseComponent
{
    public Button button;
    public TextMeshProUGUI textChoice;
    public bool _hasInteract = false;
}
