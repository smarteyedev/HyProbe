using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public abstract class ABaseNPCController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [Header("NPC Data")]
        [SerializeField] private NPCStatus _npcStatus;
        [SerializeField] private DialogController _dialog;
        [Range(0, 100)]
        public float moodValue;

        [Header("References Component")]
        [SerializeField] private ObjectCharacteristicData characteristicData;


        [Header("QuestData")]
        [SerializeField] private DialogGraphController _dialogData;
        [SerializeField] private AnswerController _answerController;
        [SerializeField] private List<QuestionData> _questions;
        [SerializeField] private int _maxAttempt;

        [SerializeField] private List<AnswerSolution> _solutions;
        [SerializeField] private string _startingDialog;

        public void SpawnNPC()
        {
            AudioManager.instance.PlaySpawnHologram();
            gameObject.SetActive(true);
            GameplayManager.instance.SetActiveNPC(this);
            _dialog.ShowDialog(_dialogData, _questions, _maxAttempt, this);
            UIGameplayManager.instance.SetNPCTextName(_npcStatus.npcName);
        }

        public float UpdateMoodValue(float value)
        {
            moodValue += value;

            return moodValue;
        }

        public void ShowAnswer()
        {
            _answerController.InitAnswer(_solutions, _startingDialog, _npcStatus.npcSprite);
        }

        public ObjectCharacteristicData GetNPCCharacteristicData()
        {
            return characteristicData;
        }

        public void PlayAnimationTalking(float mood)
        {
            if(mood >= 0)
            {
                _animator.SetTrigger("Talking1");
            }
            else
            {
                _animator.SetTrigger("Talking2");
            }
        }

        public void PlayFadeoutAnimation()
        {
            _animator.SetTrigger("Fadeout");
        }
    }
}

[System.Serializable]
public class NPCStatus
{
    public string npcName;
    public string npcDescription;
    public Sprite npcSprite;
    public float trust;
    public float social;
    public float intelligence;
    public float funny;
}

[System.Serializable]
public class AnswerSolution
{
    public string answerText;
    public float answerPoint;
    public float moodPoint;
    public string endText;

    [TextArea(0, 5)]
    public string ending;
}
