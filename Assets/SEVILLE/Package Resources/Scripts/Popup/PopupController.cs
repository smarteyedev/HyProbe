using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Seville
{
    public class PopupController : MonoBehaviour
    {
        private PopupData popupDataTemp;

        [Header("Component Dependencies")]
        [SerializeField] private PopupConfiguration popupConfig;

        [Space(4f)]
        [SerializeField] private GameObject _panelMain;
        [SerializeField] private Button _actionButton;
        private bool isOpen = false;

        private void Start()
        {
            _actionButton.onClick.AddListener(OpenPopupPanel);
        }

        public void OpenPopupPanel()
        {
            if (!isOpen)
            {
                isOpen = true;
                UIAnimator.ScaleInObject(
                        _panelMain
                    );
            }
            else
            {
                isOpen = false;
                UIAnimator.ScaleOutObject(
                        _panelMain
                    );
            }
        }
    }
}