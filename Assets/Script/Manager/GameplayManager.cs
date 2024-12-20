using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SR
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager instance;

        [Header("Spawner Properties")]
        [SerializeField] private NPCSpawnerManager _npcSpawner;

        [Header("Attempt Properties")]
        [SerializeField] private int _currentAttempQuestion = 0;
        [SerializeField] private List<GameObject> _numberObjects = new List<GameObject>();
        private int _previousAttemptModel = 0;

        [Header("Player Progres")]
        private int _level = 0;

        [Header("QuestData")]
        public List<QuestionData> _activeQuestions = new List<QuestionData>();
        public int maxAttempt;


        [Header("NPC")]
        public ABaseNPCController _activeNPC = null;

        private void Awake()
        {
            instance = this;
        }

        public void ClearStage()
        {
            _level++;
        }

        public void ResetAttempt()
        {
            foreach (GameObject obj in _numberObjects)
            {
                obj.SetActive(false);
            }

            _currentAttempQuestion = 0;
            SetAttemptNumberModel(_currentAttempQuestion, true);
        }

        public void OnAttemptQuestion(int attempt)
        {
            _previousAttemptModel = _currentAttempQuestion;
            SetAttemptNumberModel(_previousAttemptModel, false);
            _currentAttempQuestion = attempt;
            SetAttemptNumberModel(_currentAttempQuestion, true);
        }

        private void SetAttemptNumberModel(int index, bool visible)
        {
            if(_numberObjects[index] != null)
            {
                _numberObjects[index].SetActive(visible);
            }
        }

        public void StartQuestion()
        {

        }

        public void SetActiveNPC(ABaseNPCController npc)
        {
            _activeNPC = npc;
        }

        public void PlayFadeoutNPC()
        {
            _activeNPC.PlayFadeoutAnimation();
            AudioManager.instance.PlaySpawnHologram();
        }

        public void PlayTalkingNPC(float effect)
        {
            _activeNPC.PlayAnimationTalking(effect);
        }

        public void SetActiveNPC(bool condition)
        {
            _activeNPC.gameObject.SetActive(condition);
        }

    }
}
