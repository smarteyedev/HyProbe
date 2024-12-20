
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace Seville
{
    public class ScoreController : MonoBehaviour
    {
        //!! public DataManager dataManager;
        public HeadCanvasController headCanvas;
#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField] private int score;
        private int scoreMax;
        public TextMeshProUGUI scoreText;
        private bool isScoreMaxReached = false;
        [Space]
        public UnityEvent OnScoreFinished;

        private void Start()
        {
            if (headCanvas)
            {
                GetDataManager();
            }
        }

        void GetDataManager()
        {
            //!!!! dataManager = headCanvas.dataManager;
            //!!score = dataManager.currentPlayerScore;
            //!! scoreMax = dataManager.playerMaxScore;
        }

        public void IncreaseScore(int val)
        {
            //!! if (!dataManager)
            //!!     GetDataManager();

            if (isScoreMaxReached)
            {
                Debug.Log("Score max reached, cannot add more score.");
                return;
            }

            score += val;

            UpdateScore();
            CheckScore();
        }

        private void FinishScore() => OnScoreFinished.Invoke();

        public void DecreaseScore(int val)
        {
            //!! if (!dataManager)
            //!!     GetDataManager();

            if (isScoreMaxReached)
            {
                Debug.Log("Score max reached, cannot subtract score.");
                return;
            }

            score -= val;
            if (score < 0)
            {
                score = 0;
            }

            UpdateScore();
            CheckScore();
        }

        public void ResetScore()
        {
            this.score = 0;

            UpdateScore();
        }

        private void CheckScore()
        {
            if (score >= scoreMax && !isScoreMaxReached)
            {
                isScoreMaxReached = true;
                Invoke(nameof(FinishScore), .2f);
            }
        }

        public int GetCurrentScore()
        {
            return score;
        }

        private void UpdateScore()
        {
            //!! if (dataManager)
            //!! {
            //!!     dataManager.currentPlayerScore = this.score;
            //!! }

            if (scoreText != null)
            {
                scoreText.text = "Score: " + this.score.ToString();
            }
        }
    }
}