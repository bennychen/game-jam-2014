using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
	private void OnEnable() { }

    public override void OnInspectorGUI()
    {
        Level level = target as Level;

        DrawDefaultInspector();

        EditorGUILayout.Separator();
        if (GUILayout.Button("更新关卡组件集合列表 Update Component Group",GUILayout.Height(30)))
        {
            UpdateCommonentGroup(level);
        }
        EditorUtility.SetDirty(level);
    }

    private void UpdateCommonentGroup(Level level)
    {
        level.ComponentGroups.Clear();
        foreach(LevelElement item in level.Elements)
        {
            if (!level.ComponentGroups.Contains(item.ComponentGroup))
            {
                level.ComponentGroups.Add(item.ComponentGroup);
            }
        }
    }
}
