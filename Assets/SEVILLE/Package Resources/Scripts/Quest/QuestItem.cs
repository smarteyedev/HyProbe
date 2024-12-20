using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Seville
{
    public class QuestItem : MonoBehaviour
    {
        [Header("Asset UI")]
        public List<SpriteAsset> checkBoxSpriteList;
        [Space(4f)]
        public List<SpriteAsset> rowBackgroundSpriteList;
        public Sprite iconDefaultSprite;

        [Header("Component Dependencies")]
        [SerializeField] private Image imgFrame;
        [SerializeField] private Image imgIcon;
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textDescription;
        [SerializeField] private Image imgState;

        public void SetupQuestItem(Sprite _icon, string _title, string _desc, bool _state)
        {
            imgFrame.sprite = _state ? rowBackgroundSpriteList.FirstOrDefault((x) => x.name == "bgActive").sprite :
                                    rowBackgroundSpriteList.FirstOrDefault((x) => x.name == "bgNonActive").sprite;
            imgIcon.sprite = _icon != null ? _icon : iconDefaultSprite;
            textTitle.text = _title;
            textDescription.text = _desc;
            imgState.sprite = _state ? checkBoxSpriteList.FirstOrDefault((x) => x.name == "boxChecked").sprite :
                                    checkBoxSpriteList.FirstOrDefault((x) => x.name == "boxUncheck").sprite;
        }
    }

    [Serializable]
    public class SpriteAsset
    {
        public string name;
        public Sprite sprite;
    }
}