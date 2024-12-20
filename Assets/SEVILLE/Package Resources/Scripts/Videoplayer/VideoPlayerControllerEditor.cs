using UnityEditor;

namespace Seville
{
#if UNITY_EDITOR
    [CustomEditor(typeof(VideoPlayerController))]
    public class VideoPlayerControllerEditor : Editor
    {
        SerializedProperty videoConfigProp;
        SerializedProperty videoplayerProp;
        SerializedProperty panelMainProp;
        SerializedProperty textTitleVideoProp;
        SerializedProperty panelThumbnailProp;
        SerializedProperty buttonOpenPanelMainProp;
        SerializedProperty buttonClosePanelMainProp;
        SerializedProperty controllerGroupProp;
        SerializedProperty buttonPlayProp;
        SerializedProperty buttonPauseProp;
        SerializedProperty buttonReverseProp;
        SerializedProperty buttonForwardProp;
        SerializedProperty buttonReplayProp;
        SerializedProperty thumbnailProp;


        private bool showComponentDependencies = false;

        private void OnEnable()
        {
            videoConfigProp = serializedObject.FindProperty("videoConfig");

            videoplayerProp = serializedObject.FindProperty("videoplayer");
            panelMainProp = serializedObject.FindProperty("panelMain");
            textTitleVideoProp = serializedObject.FindProperty("textTitleVideo");
            panelThumbnailProp = serializedObject.FindProperty("panelThumbnail");
            buttonOpenPanelMainProp = serializedObject.FindProperty("buttonOpenPanelMain");
            buttonClosePanelMainProp = serializedObject.FindProperty("buttonClosePanelMain");
            controllerGroupProp = serializedObject.FindProperty("controllerGroup");
            buttonPlayProp = serializedObject.FindProperty("buttonPlay");
            buttonPauseProp = serializedObject.FindProperty("buttonPause");
            buttonReverseProp = serializedObject.FindProperty("buttonReverse");
            buttonForwardProp = serializedObject.FindProperty("buttonForward");
            buttonReplayProp = serializedObject.FindProperty("buttonReplay");
            thumbnailProp = serializedObject.FindProperty("defaultThumbnailSprite");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(videoConfigProp);

            EditorGUILayout.Space(5f);
            // Toggle showComponents
            showComponentDependencies = EditorGUILayout.Toggle("Show Component Dependencies", showComponentDependencies);

            if (showComponentDependencies)
            {
                EditorGUILayout.PropertyField(videoplayerProp);
                EditorGUILayout.PropertyField(panelMainProp);
                EditorGUILayout.PropertyField(textTitleVideoProp);
                EditorGUILayout.PropertyField(panelThumbnailProp);
                EditorGUILayout.PropertyField(buttonOpenPanelMainProp);
                EditorGUILayout.PropertyField(buttonClosePanelMainProp);
                EditorGUILayout.PropertyField(controllerGroupProp);
                EditorGUILayout.PropertyField(buttonPlayProp);
                EditorGUILayout.PropertyField(buttonPauseProp);
                EditorGUILayout.PropertyField(buttonReverseProp);
                EditorGUILayout.PropertyField(buttonForwardProp);
                EditorGUILayout.PropertyField(buttonReplayProp);
                EditorGUILayout.PropertyField(thumbnailProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}