using UnityEditor;
using UnityEngine;

namespace Seville
{
#if UNITY_EDITOR
    // [CustomEditor(typeof(ScoreController))]
    public class ScoreControllerEditor : Editor
    {
        SerializedProperty dataManagerProperty;

        private void OnEnable()
        {
            // Dapatkan properti dari DataManagerHandler untuk mengontrol bagaimana ia ditampilkan di Editor
            dataManagerProperty = serializedObject.FindProperty("dataManager");
        }

        public override void OnInspectorGUI()
        {
            // Render properti lain dengan DrawDefaultInspector
            serializedObject.Update();

            // Susun GUI secara manual sesuai keinginan Anda:

            // 1. Render properti dataManager dan tombol
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(dataManagerProperty, true);

            if (dataManagerProperty.objectReferenceValue == null)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace(); // Ini akan mendorong tombol ke kanan
                if (GUILayout.Button("Create DataManager", GUILayout.Width(250), GUILayout.Height(20)))
                {
                    //!!! DataManager newDataManager = CreateDataManagerAsset();
                    //!!! dataManagerProperty.objectReferenceValue = newDataManager;
                    serializedObject.ApplyModifiedProperties();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(15f);

            // 2. Render properti lain setelah dataManager
            DrawPropertiesExcluding(serializedObject, "dataManager");

            serializedObject.ApplyModifiedProperties();
        }

        //!! private DataManager CreateDataManagerAsset()
        //! {
        //!     DataManager asset = ScriptableObject.CreateInstance<DataManager>();

        //     // Tentukan di mana Anda ingin menyimpan asset DataManager ini.
        // !    AssetDatabase.CreateAsset(asset, "Assets/SEVILLE/MyDataManager.asset");
        // !    AssetDatabase.SaveAssets();

        // !    return asset;
        //! }
    }
#endif
}