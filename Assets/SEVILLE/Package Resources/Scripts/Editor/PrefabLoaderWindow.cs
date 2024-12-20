using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using Unity.XR.CoreUtils;

namespace Seville
{
    public class PrefabLoaderWindow : EditorWindow
    {
        private Dictionary<string, List<GameObject>> prefabsByFolder = new Dictionary<string, List<GameObject>>();
        private string currentFolderPath;

        // [MenuItem("Seville/Asset Prefabs Group")]
        // public static void ShowWindowPrefabs()
        // {
        //     var window = EditorWindow.GetWindow<PrefabLoaderWindow>();
        //     window.LoadPrefabs("Assets/_Sandboxing/Prefabs");
        //     window.currentFolderPath = "Assets/_Sandboxing/Prefabs";
        //     window.titleContent = new GUIContent("Smarteye Virtual Learning | Asset Prefabs Group");
        // }

        // [MenuItem("Seville/Environment 3D")]
        // public static void ShowWindowPrefabs2()
        // {
        //     var window = EditorWindow.GetWindow<PrefabLoaderWindow>();
        //     window.LoadPrefabs("Assets/SEVILLE/Package Resources/Prefabs/Tesing/3D");
        //     window.currentFolderPath = "Assets/SEVILLE/Package Resources/Prefabs/Tesing/3D";
        //     window.titleContent = new GUIContent("Environment 3D");
        // }

        private void LoadPrefabs(string folderPath)
        {
            var prefabPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
            prefabsByFolder.Clear();

            foreach (var path in prefabPaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var directory = Path.GetDirectoryName(path).Replace("\\", "/");
                var folderName = directory.Split('/').Last();

                if (!prefabsByFolder.ContainsKey(folderName))
                {
                    prefabsByFolder[folderName] = new List<GameObject>();
                }

                prefabsByFolder[folderName].Add(prefab);
            }
        }

        void OnGUI()
        {
            GUILayout.Label($"Available Prefabs from '{currentFolderPath}'", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (prefabsByFolder.Count > 0)
            {
                foreach (var pair in prefabsByFolder)
                {
                    var style = new GUIStyle(EditorStyles.largeLabel) { fontStyle = FontStyle.Bold, fontSize = 15 };
                    GUILayout.Label(pair.Key, style);
                    GUILayout.Space(10);

                    foreach (var prefab in pair.Value)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(prefab.name, GUILayout.Width(200));

                        if (GUILayout.Button("Add to Scene"))
                        {
                            PrefabUtility.InstantiatePrefab(prefab);
                        }

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(20);
                }
            }
            else
            {
                GUILayout.Label("No prefabs found.");
            }
        }

        [MenuItem("GameObject/Seville/Create Project VR 360", false, 10)]
        private static void InstanceStarterProject360()
        {
            GameObject prefab1 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Character/CHARACTER CONTROLLER VR 360.prefab");
            GameObject prefab2 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Environments/ENVIRONMENT MANAGER VR 360.prefab");
            GameObject prefab3 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Audio/AUDIO MANAGER.prefab");
            GameObject prefab4 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Canvas/HEAD CANVAS.prefab");
            GameObject prefab5 = AddPackage("Assets/SEVILLE/Plugins/XR Interaction Toolkit/2.4.3/XR Device Simulator/XR Device Simulator.prefab");

            if (prefab1 != null && prefab2 != null)
            {
                XROrigin origin = prefab1.GetComponentInChildren<XROrigin>();
                EnvironmentManager env = prefab2.GetComponent<EnvironmentManager>();

                if (origin != null && env != null)
                {
                    env.characterOrigin = origin;

                    Debug.Log($"Starter asset for project VR 360 has been added");
                }

                Camera cam = prefab1.GetComponentInChildren<Camera>();
                HeadCanvasController head = prefab4.GetComponent<HeadCanvasController>();

                if (cam != null && head != null)
                {
                    head.playerHead = cam.transform;
                }
            }
        }

        [MenuItem("GameObject/Seville/Create Project VR 3D", false, 10)]
        private static void InstanceStarterProjectVR3D()
        {
            GameObject prefab1 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Environments/ENVIRONMENT AREA MANAGER VR 3D.prefab");
            GameObject prefab2 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Audio/AUDIO MANAGER.prefab");
            GameObject prefab3 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Character/CHARACTER CONTROLLER VR 3D.prefab");
            GameObject prefab4 = AddPackage("Assets/SEVILLE/Package Resources/Prefabs/Canvas/HEAD CANVAS.prefab");
            GameObject prefab5 = AddPackage("Assets/SEVILLE/Plugins/XR Interaction Toolkit/2.4.3/XR Device Simulator/XR Device Simulator.prefab");

            if (prefab1 != null && prefab2 != null)
            {
                Camera cam = prefab3.GetComponentInChildren<Camera>();
                HeadCanvasController head = prefab4.GetComponent<HeadCanvasController>();

                if (cam != null && head != null)
                {
                    head.playerHead = cam.transform;

                    Debug.Log($"Starter asset for project VR 3D has been added");
                }
            }
        }

        //?? [MenuItem("GameObject/Seville/Create Project Multiplayer", false, 10)]
        //?? private static void InstanceStarterProjectMultiplayer(){}

        private static GameObject AddPackage(string prefabPath)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = $"---- {prefab.name} ----";

                return obj;
            }
            return null;
        }
    }
}