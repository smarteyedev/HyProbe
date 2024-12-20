using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Seville;

[CustomEditor(typeof(CarouselConfiguration))]
public class CarouselConfigurationEditor : Editor
{
    private bool showEvents = false;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Menampilkan m_Script dan menjadikannya read-only
        SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");
        EditorGUI.BeginDisabledGroup(true); // Membuat field menjadi read-only
        EditorGUILayout.PropertyField(scriptProperty);
        EditorGUI.EndDisabledGroup();

        // Iterasi semua property lainnya
        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true);

        while (property.NextVisible(false))
        {
            if (property.name == "onShowPanel" ||
                property.name == "onCompleteShowAllPanel" ||
                property.name == "setShowFirstTimeOnly" ||
                property.name == "setCompleteFirstTimeOnly")
                continue; // Lewati properti yang tidak ingin ditampilkan

            EditorGUILayout.PropertyField(property, true);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Event Configuration", EditorStyles.boldLabel);
        showEvents = EditorGUILayout.Foldout(showEvents, "Carousel Events");

        if (showEvents)
        {
            // Cek apakah properti ada sebelum mencoba menampilkannya
            SerializedProperty onFirstShowPanelProp = serializedObject.FindProperty("onShowPanel");
            SerializedProperty setShowFirstTimeOnlyProp = serializedObject.FindProperty("setShowFirstTimeOnly");
            SerializedProperty onCompleteShowAllPanelProp = serializedObject.FindProperty("onCompleteShowAllPanel");
            SerializedProperty setCompleteFirstTimeOnlyProp = serializedObject.FindProperty("setCompleteFirstTimeOnly");


            if (setShowFirstTimeOnlyProp != null)
            {
                EditorGUILayout.PropertyField(setShowFirstTimeOnlyProp);
            }
            if (onFirstShowPanelProp != null)
            {
                EditorGUILayout.PropertyField(onFirstShowPanelProp);
            }

            EditorGUILayout.Space();

            if (setCompleteFirstTimeOnlyProp != null)
            {
                EditorGUILayout.PropertyField(setCompleteFirstTimeOnlyProp);
            }
            EditorGUILayout.Space();
            if (onCompleteShowAllPanelProp != null)
            {
                EditorGUILayout.PropertyField(onCompleteShowAllPanelProp);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}



[CustomEditor(typeof(CarouselController))]
public class CarouselControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");
        EditorGUI.BeginDisabledGroup(true); // Membuat field menjadi read-only
        EditorGUILayout.PropertyField(scriptProperty);
        EditorGUI.EndDisabledGroup();

        CarouselController config = (CarouselController)target;

        // Mulai menggambar custom inspector
        serializedObject.Update();

        // Menampilkan semua properti di luar grup yang dapat dikontrol
        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true); // Memulai iterasi, skip "m_Script"

        while (property.NextVisible(false))
        {
            // Hanya tampilkan properti yang bukan bagian dari grup
            if (property.name != "editCarouselController" && property.name != "editButtonController" && property.name != "editDotsController" &&
                property.name != "_displayImage" && property.name != "_textDescription" && property.name != "_dotContainer" && property.name != "_dotPrefab" && property.name != "_bodyCanvasGroup" &&
                property.name != "_buttonNext" && property.name != "_buttonPrevious" && property.name != "_closeButton" && property.name != "_hoverColor" && property.name != "_defaultColor" &&
                property.name != "_hoverClipName" && property.name != "_pressedClipName" && property.name != "_dotsDefaultColor" && property.name != "_dotsActiveColor")
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        EditorGUILayout.Space();

        // === Carousel Controller ===
        config.editCarouselController = EditorGUILayout.Toggle("Edit Carousel Controller", config.editCarouselController);
        if (config.editCarouselController)
        {
            DrawProperties("_displayImage", "_textDescription", "_dotContainer", "_dotPrefab", "_bodyCanvasGroup");
        }

        EditorGUILayout.Space();

        // === Button Controller ===
        config.editButtonController = EditorGUILayout.Toggle("Edit Button Controller", config.editButtonController);
        if (config.editButtonController)
        {
            DrawProperties("_buttonNext", "_buttonPrevious", "_closeButton", "_hoverColor", "_defaultColor", "_hoverClipName", "_pressedClipName");
        }

        EditorGUILayout.Space();

        // === Dots Controller ===
        config.editDotsController = EditorGUILayout.Toggle("Edit Dots Controller", config.editDotsController);
        if (config.editDotsController)
        {
            DrawProperties("_dotsDefaultColor", "_dotsActiveColor");
        }

        serializedObject.ApplyModifiedProperties();
    }

    // Metode untuk menggambar properti 
    private void DrawProperties(params string[] properties)
    {
        for (var i = 0; i < properties.Length; i++)
        {
            SerializedProperty property = serializedObject.FindProperty(properties[i]);
            if (property != null)
            {
                EditorGUILayout.PropertyField(property, new GUIContent(property.displayName));
            }
        }
    }
}
