using System;
using UnityEngine.Events;
using UnityEngine;

namespace Seville
{
    public class PopupConfiguration : MonoBehaviour
    {
        [Header("Contents")]
        [SerializeField] private PopupData popupDatas;

        [Header("Settings")]
        public bool isOpenOnStart;

        [Header("Quest Event")]
        public PopupEvent UnityEvents;

        public PopupData GetPopupData()
        {
            return new PopupData(popupDatas);
        }
    }

    [Serializable]
    public class PopupEvent
    {
        [Space(2f)]
        [Tooltip("The OnQuestOpen event will be called once the quest has opened.")]
        public UnityEvent OnQuestOpen;
        [Space(4f)]
        [Tooltip("The OnQuestFinished event will be called after the player finishes all item tasks.")]
        public UnityEvent OnQuestFinished;
    }

    [Serializable]
    public class PopupData
    {
        public string titleText;
        public Sprite contentSprite;
        public string descriptionText;

        public PopupData(PopupData other)
        {
            titleText = other.titleText;
            contentSprite = other.contentSprite;
            descriptionText = other.descriptionText;
        }
    }
}