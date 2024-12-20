using SR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SR
{
    public class NPCSpawnerManager : MonoBehaviour
    {
        [SerializeField] private GameObject _selectorParent;
        [SerializeField] private List<NPCControllerData> _listNPC;

        [Header("Selector Properties")]
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _selectedColor;
        private Coroutine _hoverCoroutine = null;
        [SerializeField] private float _rotationDuration = 5f;

        private int _selectedIndex = 0;
        private bool _hasSelected = false;

        [Header("UI Component")]
        [SerializeField] private GameObject _panelSelctor;

        private void Start()
        {
            ResetAllSelected();
        }

        public void ShowSelectorMenu(bool condition)
        {
            _selectorParent.SetActive(condition);

            if (!condition)
                return;

            foreach(NPCControllerData data in _listNPC)
            {
                if (data.isClear)
                {
                    data.npcHead.SetActive(false);
                    data.segitigaHologram.SetActive(false);
                }
            }
        }

        public void SpawnNPC()
        {
            if (!_hasSelected)
            {
                UIGameplayManager.instance.ShowNotification("Pilih Dahulu Pelanggan");
            }
            else
            {
                _listNPC[_selectedIndex].npcController.SpawnNPC();
                _listNPC[_selectedIndex].isClear = true;
                _hasSelected = false;
                ResetAllSelected();
                ShowSelectorMenu(false);
                //ObjectInteractionController.instance.HideObjectInteraction();


                UIGameplayManager.instance.ShowSelectorPanel(false);
                UIGameplayManager.instance.ShowDialogPanel(true);
                UIGameplayManager.instance.ShowSliderMood(true);
            }
        }

        public void ClearStageNPC()
        {
            _listNPC[_selectedIndex].isClear = true;
            ResetAllSelected();
            ShowSelectorMenu(true);
        }

        public void OnEnterHoverHeadNPC(Transform obj)
        {
            if(_hoverCoroutine != null)
            {
                OnExitHoverHeadNPC();
                _hoverCoroutine = StartCoroutine(RotateContinuously(obj));
            }
            else
            {
                _hoverCoroutine = StartCoroutine(RotateContinuously(obj));
            }
        }

        public void OnExitHoverHeadNPC()
        {
            if (_hoverCoroutine != null)
            {
                StopCoroutine(_hoverCoroutine);
                _hoverCoroutine = null;
            }
        }

        private IEnumerator RotateContinuously(Transform obj)
        {
            while (true)
            {
                float elapsedTime = 0f;
                // Ambil sudut awal berdasarkan rotasi saat ini pada sumbu Y
                float initialAngle = obj.eulerAngles.y;
                float targetAngle = initialAngle + 360f; // Tentukan target rotasi (360 derajat dari sudut awal)

                while (elapsedTime < _rotationDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float progress = elapsedTime / _rotationDuration;

                    // Interpolasi rotasi dari sudut awal ke target sudut
                    float currentAngle = Mathf.Lerp(initialAngle, targetAngle, progress);
                    obj.rotation = Quaternion.Euler(0, currentAngle, 0);

                    yield return null; // Tunggu frame berikutnya
                }

                // Pastikan rotasi berakhir di targetAngle
                obj.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }

        public void SelectNPC(int index)
        {
            ResetAllSelected();
            _hasSelected = true;

            _selectedIndex = index;
            _listNPC[_selectedIndex].headMaterial.SetColor("_MainColor", _selectedColor);
            ObjectInteractionController.instance.SetCharacteristicNPC(_listNPC[_selectedIndex].npcController.GetNPCCharacteristicData());
        }

        private void ResetAllSelected()
        {
            _hasSelected = false;
            for (int i = 0; i < _listNPC.Count; i++)
            {
                _listNPC[i].headMaterial.SetColor("_MainColor", _baseColor);
            }
        }
    }
}

[System.Serializable]
public class NPCControllerData
{
    public ABaseNPCController npcController;
    public GameObject npcHead;
    public GameObject segitigaHologram;
    public Material headMaterial;
    public bool isClear = false;
}
