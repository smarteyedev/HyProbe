using Seville;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;


namespace SR
{
    public class ResultController : MonoBehaviour
    {
        [Header("UI Component")]
        [SerializeField] private GameObject _panelResult;
        [SerializeField] private TextMeshProUGUI _textNPCName;
        [SerializeField] private TextMeshProUGUI _textEnding;
        [SerializeField] private TextMeshProUGUI _textResult;
        [SerializeField] private Slider _sliderMood;
        [SerializeField] private Slider _sliderResult;

        public void SetUIResult(ResultData resultData)
        {
            UIGameplayManager.instance.ShowNextPanel(true);
            GameplayManager.instance.SetActiveNPC(false);
            AudioManager.instance.PlayPopupButon();

            OpenResultPanel(true);
            _textEnding.text = resultData.textEnding;
            _textResult.text = resultData.resultPoint.ToString();

            _sliderResult.value = _sliderMood.value;
        }

        public void OpenResultPanel(bool condition)
        {
            if (condition)
            {
                UIAnimator.ScaleInObject(
                        _panelResult
                    );
            }
            else
            {
                UIAnimator.ScaleOutObject(
                        _panelResult
                    );
            }
        }
    }
}

public class ResultData
{
    public string npcName;
    public string textEnding;
    public float resultPoint;
    public float moodValue;
    public Sprite npcSprite;
}
