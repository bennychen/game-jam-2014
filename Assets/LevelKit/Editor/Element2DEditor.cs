using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Element2D))]
public class Element2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Element2D element = target as Element2D;

        GUI.changed = false;

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Type", EditorStyles.boldLabel);
        element.Type = (Element2DType)EditorGUILayout.EnumPopup(element.Type);

        if (element.ChildElements.Count > 0 && element.Type != Element2DType.Group)
        {
            Debug.LogError("You must firstly remove all the child elements to make [" + element.name + "] a non-group element");
            element.Type = Element2DType.Group;
        }

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Bounds", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Min: " + element.Min + ", Max: " + element.Max, EditorStyles.numberField);

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Parent", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(element.Parent ? element.Parent.name : "No Parent");

        if (element.Type == Element2DType.Group)
        {
            EditorGUILayout.Separator();
            SerializedProperty property = serializedObject.FindProperty("ChildElements");
            EditorGUILayout.BeginVertical();
            do
            {
                EditorGUILayout.PropertyField(property);
            }
            while (property.NextVisible(true));
            EditorGUILayout.EndVertical();
        }

        if (element.Parent == null && GUILayout.Button("Rebuild", GUILayout.Height(30)))
        {
            element.Rebuild();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(element);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private Vector2 _scrollPosition;
}