using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelComponent))]
public class LevelComponentEditor : Editor {

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LevelComponent component = target as LevelComponent;

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("ID",component.ID.ToString());

        EditorGUILayout.Separator();
        component.MinDepth = 
            EditorGUILayout.FloatField("最小深度 MinDepth", component.MinDepth);

        EditorGUILayout.Separator();
        component.MaxDepth = 
            EditorGUILayout.FloatField("最大深度 MaxDepth", component.MaxDepth);

        EditorGUILayout.Separator();
        float currentDepth =
            EditorGUILayout.FloatField("当前深度 Depth", component.transform.position.z);
        Vector3 position = component.transform.position;
        if (currentDepth <= component.MaxDepth && currentDepth >= component.MinDepth)
        {
            position.z = currentDepth;
            component.transform.position = position;
        }

        position.z = EditorGUILayout.Slider(position.z, component.MinDepth, component.MaxDepth);
        component.transform.position = position;

        EditorGUILayout.Separator();
        component.IsScalable =
            EditorGUILayout.Toggle("可缩放 IsScalable", component.IsScalable);

        EditorGUILayout.Separator();
        component.SnapToVerticalLine =
            EditorGUILayout.Toggle("吸附到竖线上 SnapHorizontally", component.SnapToVerticalLine);

        EditorGUILayout.Separator();
        component.SnapToHorizontalLine =
            EditorGUILayout.Toggle("吸附到横线上 SnapVertically", component.SnapToHorizontalLine);

        EditorGUILayout.Separator();
        component.PoolStartCount =
            EditorGUILayout.IntField("Pool Init Count", component.PoolStartCount);

        EditorGUILayout.Separator();
        component.IsHighEndOnly =
            EditorGUILayout.Toggle("只在高端机上创建", component.IsHighEndOnly);
        EditorGUILayout.LabelField("Is High-end Only");

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Static Colliders", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Count: " + component.StaticColliders.Count, EditorStyles.label);
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        for (int i = 0; i < component.StaticColliders.Count; i++)
        {
            Collider collider = component.StaticColliders[i];
            if (collider != null)
            {
                EditorGUILayout.LabelField((i + 1) + ": " +
                    (collider.gameObject == component.gameObject ? "self" : collider.name), EditorStyles.numberField);
            }
            else
            {
                EditorGUILayout.LabelField((i + 1) + ": null", EditorStyles.numberField);
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("组件所属集合 Level Component Group", EditorStyles.boldLabel);
        EditorGUILayout.ObjectField(component.ComponentGroup, typeof(LevelComponentGroup), false);

        EditorGUILayout.Separator();
        if (GUILayout.Button("Rebuild", GUILayout.Height(30)))
        {
            component.Rebuild();
        }
        serializedObject.ApplyModifiedProperties();
    }

    protected void OnEnable()
    {
        TrySnapToGrid(target as LevelComponent);
    }

    protected void OnSceneGUI()
    {
        if (Event.current != null && Selection.activeGameObject != null)
        {
            if (Event.current.type == EventType.MouseUp)
            {
                foreach (Transform transform in Selection.transforms)
                {
                    LevelComponent component = transform.GetComponent<LevelComponent>();
                    TrySnapToGrid(component);
                }
            }
        }
    }

    protected void TrySnapToGrid(LevelComponent component)
    {
        component.DoSnapToGrid();
    }

    private Vector2 _scrollPosition;
}