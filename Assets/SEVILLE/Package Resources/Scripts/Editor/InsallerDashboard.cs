#if UNITY_EDITOR
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
#endif

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Seville
{
#if UNITY_EDITOR
    public class InsallerDashboard : EditorWindow
    {
        private static List<string> dependenciesList = new List<string>() { "com.unity.xr.interaction.toolkit" };
        private static List<string> buttonList = new List<string>() { "", "" };

        private ListRequest listRequest;
        private AddRequest addRequest;

        static bool isChecking = false;
        private bool isInstalling = false;
        private bool isButtonUpdate = false;

        float packagesDownloadedCount = 0;

        // Tags and Layers Settings
        static List<string> tagsList = new List<string>();
        static List<string> layersList = new List<string>();



        [MenuItem("Tools/Seville/Installer Dashboard")]
        public static void OpenInstallerDashboard()
        {
            GetWindow<InsallerDashboard>("Installer Basic Package Dashboard");

            isChecking = true;
        }

        private void OnGUI()
        {
            if (isChecking)
            {
                CheckDepedenciesPackage();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Installer Progress : ", EditorStyles.boldLabel);

            InstallerProgress(packagesDownloadedCount, dependenciesList.Count, "Install Progress");

            UpdateButtons();

            if (packagesDownloadedCount < dependenciesList.Count)
                EditorGUILayout.HelpBox("You have to install all basic package before use the Seville framework", MessageType.Error);

            if (!isButtonUpdate) EditorGUILayout.LabelField("Checking...", EditorStyles.boldLabel);

            if (isInstalling)
                EditorGUILayout.LabelField("Installing...", EditorStyles.boldLabel);
        }

        void UpdateButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("XR Integration ToolKit :", EditorStyles.boldLabel);

            string nameBtn1 = buttonList[0] != "" ? buttonList[0] : "wait...";

            if (GUILayout.Button(nameBtn1))
            {
                InstallPackage(0, "2.4.3");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }

        void InstallerProgress(float value, float maxValue, string label)
        {
            float realValue = 1f / maxValue;
            float progressValue = value * realValue;

            Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
            EditorGUI.ProgressBar(rect, progressValue, label);
            EditorGUILayout.Space(10);
        }

        private void CheckDepedenciesPackage()
        {
            packagesDownloadedCount = 0;
            isButtonUpdate = false;
            // Membuat permintaan untuk mendapatkan daftar paket yang ada dalam proyek
            listRequest = Client.List();

            // Menjalankan proses instalasi setelah daftar paket tersedia
            EditorApplication.update += ResetDependenciesState;

            isChecking = false;
        }

        private void ResetDependenciesState()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    Debug.Log($"Processing....");
                    for (int i = 0; i < dependenciesList.Count; i++)
                    {
                        bool notAvailable = listRequest.Result.Any((x) => x.name == dependenciesList[i]);
                        if (!notAvailable)
                        {
                            Debug.Log($"{dependenciesList[i]} is not available in package manager");
                            buttonList[i] = "Install";
                        }
                        else
                        {
                            Debug.Log($"{dependenciesList[i]} is available");
                            buttonList[i] = "Reinstall";

                            packagesDownloadedCount += 1;
                        }
                    }

                    isButtonUpdate = true;
                }
                else
                {
                    Debug.LogError("Gagal mendapatkan daftar paket.");
                }
                // Hentikan pemantauan pembaruan
                EditorApplication.update -= ResetDependenciesState;
            }
        }

        private void InstallPackage(int _index, string _version)
        {
            isInstalling = true;

            // Create the request to add the package
            AddRequest request = Client.Add($"{dependenciesList[_index]}@{_version}");

            // Check the status of the request in the Update loop
            EditorApplication.update += OnUpdate;

            void OnUpdate()
            {
                if (request.IsCompleted)
                {
                    isInstalling = false;
                    if (request.Status == StatusCode.Success)
                    {
                        Debug.Log($"Package {dependenciesList[_index]}@{_version} installed successfully!");

                        isChecking = true;
                    }
                    else
                    {
                        Debug.LogError($"Failed to install package {dependenciesList[_index]}@{_version}: {request.Error.message}");

                        isChecking = true;
                    }

                    // Remove the update callback
                    EditorApplication.update -= OnUpdate;
                }
            }
        }
    }
#endif
}