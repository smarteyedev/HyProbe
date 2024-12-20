using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Seville
{
    public class QuestController : MonoBehaviour
    {
        // [SerializeField] 
        private List<QuestData> questDataTempList;

        [Header("Component Dependencies")]
        [SerializeField] private QuestConfiguration questConfig;
        [Space(5f)]
        [SerializeField] private QuestItem questItemPrefab;
        [SerializeField] private Transform itemParent;
        [SerializeField] private GameObject panelMain;
        [Space(3f)]
        [SerializeField] private GameObject btnOpenQuest;
        [SerializeField] private Button btnCloseQuest;

        void Awake()
        {
            if (questDataTempList != null)
                questDataTempList.Clear();
        }

        private void Start()
        {
            btnOpenQuest.GetComponent<Button>().onClick.AddListener(() => OpenQuestPanel());
            btnCloseQuest.onClick.AddListener(() => CloseQuestPanel());

            btnOpenQuest.SetActive(true);
            panelMain.SetActive(false);

            if (questConfig.isOpenOnStart) OpenQuestPanel();
        }

        public void OpenQuestPanel()
        {
            if (questDataTempList == null)
            {
                if (questConfig.GetQuestData().Count == 0)
                {
                    Debug.LogWarning($"quest data is {questConfig.GetQuestData().Count}, please fill the data in quest configuration");
                    return;
                }
                else
                {
                    questDataTempList = questConfig.GetQuestData();
                }
            }

            questConfig.UnityEvents.OnQuestOpen?.Invoke();

            UIAnimator.ScaleInObject(
                panelMain,
                () =>
                {
                    foreach (Transform child in itemParent.transform)
                    {
                        UnityEngine.Object.Destroy(child.gameObject);
                    }

                    btnOpenQuest.SetActive(false);
                },
                () =>
                {
                    StartCoroutine(PrintQuestItem());
                }
            );
        }

        private IEnumerator PrintQuestItem()
        {
            if (itemParent.childCount > 0)
            {
                foreach (Transform child in itemParent.transform)
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }

            int i = 0;
            while (i < questDataTempList.Count + 1)
            {
                if (i == questDataTempList.Count)
                {
                    // bug can't scroll in awake
                    var rowTemp = Instantiate(questItemPrefab, itemParent);
                    StartCoroutine(DestroyTemp(rowTemp.gameObject));
                }
                else
                {
                    var rowItem = Instantiate(questItemPrefab, itemParent);
                    rowItem.SetupQuestItem(questDataTempList[i].iconSprite, questDataTempList[i].taskTitleText, questDataTempList[i].taskDescriptionText, questDataTempList[i].isTaskState);
                }

                yield return new WaitForSeconds(0.01f);
                i++;
            }
        }

        IEnumerator DestroyTemp(GameObject go)
        {
            yield return new WaitUntil(() => go != null);
            Destroy(go);
            // Debug.Log($"destroy {go}");
        }

        public void CloseQuestPanel()
        {
            UIAnimator.ScaleOutObject(
                panelMain,
                () => { },
                () =>
                {
                    panelMain.SetActive(false);
                    btnOpenQuest.SetActive(true);
                }
            );
        }

        public void FinishQuestItemTask(int indexItem)
        {
            var dataTarget = questDataTempList;

            if (indexItem > dataTarget.Count - 1)
            {
                Debug.LogWarning($"number {indexItem} is out of todolist count");
                return;
            }

            if (!dataTarget[indexItem].isTaskState)
            {
                var temp = dataTarget[indexItem];
                temp.isTaskState = true;

                questDataTempList[indexItem] = temp;
            }

            if (questDataTempList.All(item => item.isTaskState))
            {
                Invoke(nameof(OnHasFinishedAllItemTask), questConfig.delayTimeOnFinished);
            }

            if (panelMain.activeSelf)
            {
                foreach (Transform child in itemParent.transform)
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
                StartCoroutine(PrintQuestItem());
            }
        }

        private void OnHasFinishedAllItemTask()
        {
            questConfig.UnityEvents.OnQuestFinished?.Invoke();
        }

        public void ResetQuestData()
        {
            questDataTempList.Clear();
        }
    }
}