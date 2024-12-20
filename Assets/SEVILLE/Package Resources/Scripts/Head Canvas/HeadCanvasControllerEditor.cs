using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace Seville
{
    [CustomEditor(typeof(HeadCanvasController))]
    public class HeadCanvasControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Ambil referensi dari objek target
            HeadCanvasController myScript = (HeadCanvasController)target;

            // Ambil semua properti yang perlu ditampilkan
            serializedObject.Update();
            SerializedProperty playerHeadProp = serializedObject.FindProperty("playerHead");
            SerializedProperty spawnDistanceProp = serializedObject.FindProperty("spawnDistance");
            SerializedProperty maxDistanceProp = serializedObject.FindProperty("maxDistance");
            SerializedProperty useMenuCanvasProp = serializedObject.FindProperty("useMenuCanvas");
            SerializedProperty MenuCanvasProp = serializedObject.FindProperty("MenuCanvas");
            SerializedProperty primaryBtnActionProp = serializedObject.FindProperty("primaryBtnAction");
            SerializedProperty UIPopupPanelProp = serializedObject.FindProperty("UI_popupPanel");
            SerializedProperty UIMessageProp = serializedObject.FindProperty("UI_message");
            SerializedProperty showNotificationTimeProp = serializedObject.FindProperty("showNotificationTime");

            // Tampilkan properti secara manual sesuai urutan yang diinginkan
            EditorGUILayout.PropertyField(playerHeadProp);
            EditorGUILayout.PropertyField(spawnDistanceProp);
            EditorGUILayout.PropertyField(maxDistanceProp);

            EditorGUILayout.PropertyField(useMenuCanvasProp);

            if (myScript.useMenuCanvas)
            {
                EditorGUILayout.PropertyField(MenuCanvasProp);
                EditorGUILayout.PropertyField(primaryBtnActionProp);
            }

            EditorGUILayout.PropertyField(UIPopupPanelProp);
            EditorGUILayout.PropertyField(UIMessageProp);
            EditorGUILayout.PropertyField(showNotificationTimeProp);

            // Terapkan perubahan ke objek asli
            serializedObject.ApplyModifiedProperties();
        }

        // this for create new data manager
        /* private DataManager CreateDataManagerAsset()
        {
            DataManager asset = ScriptableObject.CreateInstance<DataManager>();

            // Tentukan di mana Anda ingin menyimpan asset DataManager ini.
            AssetDatabase.CreateAsset(asset, "Assets/SEVILLE/My Data Manager/New Data Manager.asset");
            AssetDatabase.SaveAssets();

            return asset;
        } */
    }
}
#endif