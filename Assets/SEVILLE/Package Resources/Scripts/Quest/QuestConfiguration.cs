using System;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Seville
{
    public class QuestConfiguration : MonoBehaviour
    {
        [Header("Contents")]
        [SerializeField] private List<QuestData> questDatas;

        [Header("Settings")]
        public bool isOpenOnStart;

        [Space(3f)]
        [Tooltip("The delay time on the task is finished.")]
        public float delayTimeOnFinished = 3f;

        [Header("Quest Event")]
        public QuestEvent UnityEvents;

        public List<QuestData> GetQuestData()
        {
            List<QuestData> datas = new List<QuestData>();
            foreach (var item in questDatas)
            {
                datas.Add(new QuestData(item));
            }
            return datas;
        }
    }


    [Serializable]
    public class QuestEvent
    {
        [Space(2f)]
        [Tooltip("The OnQuestOpen event will be called once the quest has opened.")]
        public UnityEvent OnQuestOpen;
        [Space(4f)]
        [Tooltip("The OnQuestFinished event will be called after the player finishes all item tasks.")]
        public UnityEvent OnQuestFinished;
    }

    [Serializable]
    public class QuestData
    {
        public string taskTitleText;
        public Sprite iconSprite;
        [TextArea]
        public string taskDescriptionText;
#if UNITY_EDITOR
        [ReadOnly]
#endif
        public bool isTaskState;

        public QuestData(QuestData other)
        {
            this.taskTitleText = other.taskTitleText;
            this.iconSprite = other.iconSprite;
            this.taskDescriptionText = other.taskDescriptionText;
            this.isTaskState = other.isTaskState;
        }
    }
}