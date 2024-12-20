using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Seville
{
    public class QuizConfiguration : MonoBehaviour
    {
        [Header("Contents")]
        public QuizData quizData;

        [Header("Settings")]
        [Tooltip("quiz will be play on start")]
        public bool shuffleAnswerOption = false;

        [Tooltip("player can retake the quiz in another time")]
        public bool canRetakeQuiz;

        [Header("Quiz Event")]
        public QuizEvent UnityEvents;

        // OnValidate ensures that each optionList in contentList has a maximum of 4 items
        private void OnValidate()
        {
            if (quizData.contentList != null)
            {
                foreach (var content in quizData.contentList)
                {
                    if (content.optionList.Count > 4)
                    {
                        content.optionList = content.optionList.GetRange(0, 4);
                        Debug.LogWarning("Option List in MQuizContent is limited to 4 items only.");
                    }
                }
            }
        }

        public QuizData GetQuizData()
        {
            return new QuizData(quizData);
        }
    }

    [Serializable]
    public class QuizEvent
    {
        [Space(2f)]
        [Tooltip("The OnQuizOpen event will be called after the player opens the quiz.")]
        public UnityEvent OnStartQuiz;
        [Space(4f)]
        [Tooltip("The OnQuizClose event will be called after the player closes the quiz.")]
        public UnityEvent OnFinishQuiz;
        [Space(4f)]
        [Tooltip("The GetCorrectScore event will be called after the player submits the quiz.")]
        public UnityEvent<int> GetCorrectAnswer;
    }

    [Serializable]
    public class QuizData
    {
        public string quizTitle;
        public string quizInstruction;
        public List<MQuizContent> contentList;

        public QuizData(QuizData other)
        {
            quizTitle = other.quizTitle;
            quizInstruction = other.quizInstruction;
            contentList = new List<MQuizContent>();
            foreach (var content in other.contentList)
            {
                contentList.Add(new MQuizContent(content));
            }
        }

        public void ShuffleContent()
        {
            foreach (var content in contentList)
            {
                Shuffle(content.optionList);
            }
        }

        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            System.Random rnd = new System.Random();
            while (n > 1)
            {
                int k = rnd.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
    }

    [Serializable]
    public class MQuizContent
    {
        public string question;
        // public Sprite imgIllustration;
        public List<optionSection> optionList;

        [System.Serializable]
        public struct optionSection
        {
            public string answerText;

            [Tooltip("check this box if option is true...")]
            public bool validate;
        }

        [Header("Player Answer Result")]
#if UNITY_EDITOR
        [ReadOnly]
#endif
        public bool isAnswerd = false;
#if UNITY_EDITOR
        [ReadOnly]
#endif
        public int answerIndex = -1;
#if UNITY_EDITOR
        [ReadOnly]
#endif
        public string answerText;
#if UNITY_EDITOR
        [ReadOnly]
#endif
        public bool isAnswerCorrect = false;

        public MQuizContent(MQuizContent other)
        {
            question = other.question;
            optionList = new List<optionSection>(other.optionList);
            isAnswerd = other.isAnswerd;
            answerIndex = other.answerIndex;
            answerText = other.answerText;
            isAnswerCorrect = other.isAnswerCorrect;
        }
    }
}
