using UnityEditor;
using UnityEngine;

namespace Seville
{
    [CustomEditor(typeof(EnvironmentAreaManagerVR3D))]
    public class AreaManagerVR3DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10);

            EnvironmentAreaManagerVR3D manager = (EnvironmentAreaManagerVR3D)target;

            manager.propsList.RemoveAll(item => item == null);

            if (GUILayout.Button("Add Features", SevilleStyleEditor.BlueButton))
            {
                AreaManagerVR3DWindow.ShowWindow(manager);
            }
        }
    }
}