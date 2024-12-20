using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tproject.AudioManager;
using Unity.XR.CoreUtils;
using UnityEditor;

namespace Seville
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance;
        EnvAreaHandler currentArea;
        public List<EnvAreaHandler> EnvAreaHandlers;
        public XROrigin characterOrigin;
        public VR360Settings VR360Settings;

        [Header("Sphere Area Settings")]
        // public Shader formatShader;
        public Material formatMaterial;
        public GameObject targetSphereArea;

        bool isChangingProcess = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void OnApplicationQuit()
        {
            Debug.Log("Application ending after " + Time.time + " seconds");
            VR360Settings.ResetAreaIndex();
            formatMaterial.color = VR360Settings.GetDefaultMaterialColor();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                Debug.Log("Application was closed " + Time.time + " seconds");
                VR360Settings.ResetAreaIndex();
                formatMaterial.color = VR360Settings.GetDefaultMaterialColor();
            }
        }

        private void Start()
        {
            if (formatMaterial.color != VR360Settings.GetDefaultMaterialColor()) formatMaterial.color = VR360Settings.GetDefaultMaterialColor();

            StartAreaByIndex(VR360Settings.GetCurrentAreaIndex());
        }

        public void StartAreaByIndex(int index)
        {
            if (index > EnvAreaHandlers.Count)
            {
                Debug.LogWarning($"Index area {index} Doesn't available in EnvAreaHandlers List");
                return;
            }

            VR360Settings.SetCurrentAreaIndex(index);
            StartCoroutine(nameof(LoadingScreen));
        }

        IEnumerator LoadingScreen()
        {
            isChangingProcess = true;

            HideEnv();
            LeanTween.alpha(targetSphereArea, 0, 2f).setOnComplete(() => StartCoroutine(nameof(CheckState)));

            if (EnvAreaHandlers[VR360Settings.GetCurrentAreaIndex()].backsound != null) AudioManager.Instance.StartTransitionToNewMusic(EnvAreaHandlers[VR360Settings.GetCurrentAreaIndex()].backsound, 0.5f);
            // else AudioManager.Instance.StartTransitionToNewMusic("Theme", 0.5f);

            yield return new WaitUntil(() => isChangingProcess == false);

            // AudioManager.Instance.UnmuteMusic();

            characterOrigin.transform.eulerAngles = new Vector3(0f, EnvAreaHandlers[VR360Settings.GetCurrentAreaIndex()].firstCamLookRotationValue, 0f);
            characterOrigin.Camera.transform.eulerAngles = new Vector3(0f, 0f, 0f);

            LeanTween.alpha(targetSphereArea, 1, 2.5f).setOnComplete(loadedComplete);
        }

        IEnumerator CheckState()
        {
            if (currentArea)
            {
                if (currentArea.isRestartOnExitArea)
                {
                    Debug.Log($"start load area {VR360Settings.GetCurrentAreaIndex()} with load scene ");
                    AsyncOperation opration = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

                    yield return new WaitUntil(() => opration.isDone);

                    SetUpMaterial(VR360Settings.GetCurrentAreaIndex());
                }
                else
                {
                    SetUpMaterial(VR360Settings.GetCurrentAreaIndex());
                }
            }
            else
            {
                SetUpMaterial(VR360Settings.GetCurrentAreaIndex());
            }

            yield return null;
        }

        private void SetUpMaterial(int index)
        {

            // Material newMat = new Material(formatShader);
            // newMat.mainTexture = EnvAreaHandlers[index].areaTexture;
            // newMat.color = new Color(1, 1, 1, 0);

            formatMaterial.mainTexture = EnvAreaHandlers[index].areaTexture;
            formatMaterial.color = new Color(1, 1, 1, 0);

            // targetSphereArea.GetComponent<MeshRenderer>().material = newMat;
            targetSphereArea.GetComponent<MeshRenderer>().material = formatMaterial;

            Debug.Log($"Area number: {VR360Settings.GetCurrentAreaIndex()} is ready...");
            isChangingProcess = false;
        }

        private void loadedComplete()
        {
            EnvAreaHandlers[VR360Settings.GetCurrentAreaIndex()].SetActiveObjsState(true);

            currentArea = EnvAreaHandlers[VR360Settings.GetCurrentAreaIndex()];
        }

        private void HideEnv()
        {
            foreach (var item in EnvAreaHandlers)
            {
                item.SetActiveObjsState(false);
            }
        }
    }
}