using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Seville
{
    [CustomEditor(typeof(EnvAreaHandler))]
    public class EnvAreaHandlerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10);

            EnvAreaHandler manager = (EnvAreaHandler)target;

            manager.areaObjsList.RemoveAll(item => item == null);

            if (GUILayout.Button("Add Features", SevilleStyleEditor.BlueButton))
            {
                EnvAreaHandlerWindow.ShowWindow(manager);
            }
        }
    }
}