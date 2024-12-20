using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Seville
{
    [CustomEditor(typeof(EnvironmentManager))]
    public class EnvironmentManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10);

            EnvironmentManager manager = (EnvironmentManager)target;

            manager.EnvAreaHandlers.RemoveAll(item => item == null);

            if (GUILayout.Button("Add New Area", SevilleStyleEditor.GreenButton))
            {
                AddNewArea(manager);
            }
        }

#if UNITY_EDITOR
        public void AddNewArea(EnvironmentManager manager)
        {
            string prefabPath = "Assets/SEVILLE/Package Resources/Prefabs/Environments/ENVIRONMENT AREA VR 360.prefab";

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.LogError("Prefab tidak ditemukan di " + prefabPath);
                return;
            }

            GameObject obj = Instantiate(prefab);
            obj.name = $"---- AREA NUMBER: {manager.EnvAreaHandlers.Count} ----";

            EnvAreaHandler handler = obj.GetComponent<EnvAreaHandler>();

            if (handler == null)
            {
                Debug.LogError("Prefab tidak memiliki komponen ChildHandler");
                return;
            }

            manager.EnvAreaHandlers.Add(handler);
        }
#endif
    }
}