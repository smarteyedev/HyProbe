using SR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public class HistoryController : MonoBehaviour
    {
        [SerializeField] private List<HistoryPanelData> _panelDatas = new List<HistoryPanelData>();
        [SerializeField] private List<HistoryData> _historyDatas = new List<HistoryData>();

        [Header("UI Component")]
        [SerializeField] private UIComponentHistoryPanel _component;

        private void Start()
        {
            foreach (HistoryPanelData panel in _panelDatas)
            {
                panel.historyPanel.Initialize(_component);
            }
        }

        public void AddHistoryData(string question, string answer, EnumAnswerIconFeedback icon)
        {
            HistoryData historyItem = new HistoryData();

            historyItem.questionText = question;
            historyItem.answerText = answer;
            historyItem.icon = icon;

            _historyDatas.Add(historyItem);
            int index = _historyDatas.IndexOf(historyItem);

            _panelDatas[index].historyPanel.SetUIOPanel(question, answer, icon);
        }

        public void ResetHistory()
        {
            _historyDatas = new List<HistoryData>();

            foreach(HistoryPanelData panel in _panelDatas)
            {
                panel.historyPanel.gameObject.SetActive(false);
                panel.hasSet = false;
            }
        }
    }
}

[System.Serializable]
public class HistoryPanelData
{
    public HistoryPanel historyPanel;
    public bool hasSet = false;
}

[System.Serializable]
public class HistoryData
{
    public string questionText;
    public string answerText;
    public EnumAnswerIconFeedback icon;
}

public enum EnumAnswerIconFeedback
{
    Happy,
    Netral,
    Sad
}

[System.Serializable]
public class UIComponentHistoryPanel
{
    public Sprite iconHappy;
    public Sprite iconNetral;
    public Sprite iconSad;
    public Color backgroundColorHappy;
    public Color backgroundColorNetral;
    public Color backgroundColorSad;
    public Color bgAnswerColorHappy;
    public Color bgAnswerColorNetral;
    public Color bgAnswerColorSad;
}
