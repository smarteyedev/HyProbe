using UnityEngine;
using UnityEditor;
using System.IO;

namespace Seville
{
    public class AreaManagerVR3DWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private EnvironmentAreaManagerVR3D targetManager;

        public static void ShowWindow(EnvironmentAreaManagerVR3D manager)
        {
            AreaManagerVR3DWindow window = GetWindow<AreaManagerVR3DWindow>("Prefab Instantiator");
            window.targetManager = manager;
        }

        private void OnGUI()
        {
            string[] prefabPaths = {
                "Assets/SEVILLE/Package Resources/Prefabs/Logic/(LOGIC) AREA TRIGGER.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Logic/(LOGIC) INTEGERS COUNTERS.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Canvas/CAROUSEL CANVAS.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Object Interactive/DOOR INTERACTIVE.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Canvas/INPUT FIELD CANVAS (VIRTUAL KEYBOARD).prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Object Interactive/INTERACTABLE OBJECT (Cube-Base).prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Object Interactive/LADDER.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Canvas/NAVIGATION CANVAS.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Canvas/POPUP CANVAS.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Object Interactive/POKE BUTTON.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Canvas/QUEST CANVAS.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Canvas/QUIZ CANVAS.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Object Interactive/SOCKET INTERACTOR.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Object Interactive/TELEPORTATION ANCHOR.prefab",
                "Assets/SEVILLE/Package Resources/Prefabs/Canvas/VIDEOPLAYER CANVAS.prefab",
            };

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

            foreach (string prefabPath in prefabPaths)
            {
                string prefabName = Path.GetFileNameWithoutExtension(prefabPath);
                if (GUILayout.Button($"{prefabName} (+)"))
                {
                    InstantiatePrefab(prefabPath);
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void InstantiatePrefab(string path)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab)
            {
                // GameObject obj = Instantiate(prefab);
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                if (targetManager)
                {
                    obj.transform.SetParent(targetManager.transform);

                    // Mendapatkan komponen ChildHandler dari objek yang baru diinstansiasi
                    EnvironmentAreaManagerVR3D handler = obj.GetComponentInParent<EnvironmentAreaManagerVR3D>();

                    if (handler)
                    {
                        // Menambahkan handler ke dalam list
                        targetManager.propsList.Add(obj.gameObject);
                    }
                    else
                    {
                        Debug.LogWarning("Objek yang diinstansiasi tidak memiliki komponen EnvironmentAreaManagerVR3D.");
                    }
                }
                else
                {
                    Debug.LogWarning("prefab referensi tidak ditemukan.");
                }
            }
        }
    }
}