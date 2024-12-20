//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Events;

//namespace SR
//{
//    [CustomEditor(typeof(DialogGraphController))]
//    public class DialogueGraphControllerEditor : Editor
//    {
//        private SerializedProperty dialogDataProperty;

//        private void OnEnable()
//        {
//            dialogDataProperty = serializedObject.FindProperty("dialogData");
//        }

//        public override void OnInspectorGUI()
//        {
//            serializedObject.Update();

//            DialogGraphController controller = (DialogGraphController)target;
//            DialogueGraph dialogueGraph = controller.dialogData;

//            if (dialogueGraph != null)
//            {
//                EditorGUILayout.Space();
//                EditorGUILayout.LabelField("Dialogue Graph Editor", EditorStyles.boldLabel);

//                if (dialogueGraph.startingNode != null)
//                {
//                    DrawNode(dialogueGraph.startingNode, dialogueGraph.GetAllNodes());
//                }
//                else if (GUILayout.Button("Create Starting Node"))
//                {
//                    dialogueGraph.startingNode = dialogueGraph.CreateNode("Starting Dialogue");
//                }

//                foreach (var node in dialogueGraph.GetAllNodes())
//                {
//                    DrawNode(node, dialogueGraph.GetAllNodes());
//                }
//            }

//            serializedObject.ApplyModifiedProperties();
//        }

//        private void DrawNode(DialogueNode node, List<DialogueNode> allNodes)
//        {
//            EditorGUILayout.BeginVertical("box");
//            EditorGUILayout.LabelField("Dialogue Node", EditorStyles.boldLabel);

//            // Draw Dialogue Text
//            node.dialogueText = EditorGUILayout.TextField("Dialogue Text", node.dialogueText);

//            // Draw Dialog Type
//            node.type = (DialogType)EditorGUILayout.EnumPopup("Dialog Type", node.type);

//            // Conditionally show properties based on DialogType
//            if (node.type != DialogType.None)
//            {
//                if (node.type == DialogType.MoodEffect)
//                {
//                    node.moodValue = EditorGUILayout.FloatField("Mood Value", node.moodValue);
//                }

//                if (node.type == DialogType.UnityEvent || node.type == DialogType.Custom)
//                {
//                    // Draw UnityEvent using SerializedProperty
//                    if (node.onDialogChoice == null)
//                    {
//                        node.onDialogChoice = new SerializableUnityEvent();
//                    }

//                    SerializedObject nodeSerializedObject = new SerializedObject(node);
//                    SerializedProperty unityEventProp = nodeSerializedObject.FindProperty("onDialogChoice");
//                    EditorGUILayout.PropertyField(unityEventProp, new GUIContent("On Dialog Choice"), true);
//                    nodeSerializedObject.ApplyModifiedProperties();
//                }
//            }

//            // Draw Choices
//            if (node.choices != null && node.choices.Count > 0)
//            {
//                EditorGUILayout.LabelField("Choices", EditorStyles.boldLabel);

//                for (int i = 0; i < node.choices.Count; i++)
//                {
//                    Choice choice = node.choices[i];

//                    EditorGUILayout.BeginHorizontal();
//                    choice.choiceText = EditorGUILayout.TextField("Choice Text", choice.choiceText);

//                    // Dropdown to select next node
//                    int selectedIndex = allNodes.IndexOf(choice.nextNode);
//                    List<string> nodeNames = allNodes.ConvertAll(n => n.dialogueText);
//                    selectedIndex = EditorGUILayout.Popup("Next Node", selectedIndex, nodeNames.ToArray());

//                    choice.nextNode = selectedIndex >= 0 ? allNodes[selectedIndex] : null;

//                    EditorGUILayout.EndHorizontal();
//                }
//            }

//            EditorGUILayout.EndVertical();
//        }
//    }
//}
