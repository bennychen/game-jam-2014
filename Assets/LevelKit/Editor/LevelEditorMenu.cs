using Fangtang.Utils;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class LevelEditorMenu : EditorWindow
{
    [MenuItem("Fangtang/Level/Level Controller Panel 关卡控制面板")]
    private static void DoOpenLevelPanelWindow()
    {
        EditorWindow.GetWindow(typeof(LevelPanelWindow)).Show();
    }

    [MenuItem("Fangtang/Level/Create Grid 创建网格")]
    private static void DoCreateGrids()
    {
        Object gizmo = GameObject.FindObjectOfType(typeof(GridGizmos));
        if (gizmo == null)
        {
            GameObject gridGO = new GameObject("Grid");
            gridGO.AddComponent<GridGizmos>();
        }
        else
        {
            Debug.LogWarning("A grid object already exists in the scene");
        }
    }

    [MenuItem("Fangtang/Level/Snap All to Grid 吸附所有到网格")]
    private static void DoAlignAll()
    {
        object[] objects = GameObject.FindObjectsOfType(typeof(LevelComponent));
        foreach (object obj in objects)
        {
            LevelComponent component = (LevelComponent)obj;
            if (component != null)
            {
                component.DoSnapToGrid();
            }
        }
    }

    [MenuItem("Fangtang/Level/Depth/Scatter -0.001f")]
    private static void DoScatterDepthDecrease()
    {
        List<LevelComponent> selected = new List<LevelComponent>();
        foreach (Transform transform in Selection.transforms)
        {
            if (transform != null && transform.GetComponent<LevelComponent>() != null)
            {
                selected.Add(transform.GetComponent<LevelComponent>());
            }
        }
        if (selected.Count > 0)
        {
            selected.Sort(new LevelComponentHorizontalSorter());

            float startDepth = selected[0].transform.position.z;
            for (int i = 0; i < selected.Count; i++)
            {
                selected[i].transform.SetPositionZ(startDepth - i * 0.001f);
            }
        }
    }

    [MenuItem("Fangtang/Level/Depth/Scatter +0.001f")]
    private static void DoScatterDepthIncrease()
    {
        List<LevelComponent> selected = new List<LevelComponent>();
        foreach (Transform transform in Selection.transforms)
        {
            if (transform != null && transform.GetComponent<LevelComponent>() != null)
            {
                selected.Add(transform.GetComponent<LevelComponent>());
            }
        }

        if (selected.Count > 0)
        {
            selected.Sort(new LevelComponentHorizontalSorter());

            float startDepth = selected[0].transform.position.z;
            for (int i = 0; i < selected.Count; i++)
            {
                selected[i].transform.SetPositionZ(startDepth + i * 0.001f);
            }
        }
    }

    [MenuItem("Fangtang/Level/Select 选择/Select All 选择所有")]
    public static void DoSelectAll()
    {
        Selection.activeGameObject = null;

        List<UnityEngine.Object> selected = new List<UnityEngine.Object>();
        LevelComponent[] configs = GameObject.FindObjectsOfType(typeof(LevelComponent)) as LevelComponent[];
        foreach (LevelComponent config in configs)
        {
            if (config != null)
            {
                selected.Add(config.gameObject);
            }
        }
        Selection.objects = selected.ToArray() as UnityEngine.Object[];
    }

    [MenuItem("Fangtang/Level/Select 选择/Delete All 删除所有")]
    public static void DoDeleteAll()
    {
        if (EditorUtility.DisplayDialog("Delete All 删除所有?",
                        "确定删除场景中所有的关卡组件吗？", "Yes", "No"))
        {
            LevelComponent[] configs = GameObject.FindObjectsOfType(typeof(LevelComponent)) as LevelComponent[];
            foreach (LevelComponent config in configs)
            {
                if (config != null)
                {
                    GameObject.DestroyImmediate(config.gameObject);
                }
            }
        }
    }

    [MenuItem("Fangtang/Level/Select 选择/Select All on Right 选择右边所有")]
    private static void DoSelectAllOnRight()
    {
        Transform lastSelection = Selection.activeTransform;
        if (lastSelection == null)
        {
            return;
        }

        Selection.activeGameObject = null;

        List<UnityEngine.Object> selected = new List<UnityEngine.Object>();
        LevelComponent[] configs = GameObject.FindObjectsOfType(typeof(LevelComponent)) as LevelComponent[];
        foreach (LevelComponent config in configs)
        {
            if (config != null &&
                config.transform.position.x > lastSelection.position.x)
            {
                selected.Add(config.gameObject);
            }
        }
        Selection.objects = selected.ToArray() as UnityEngine.Object[];
    }

    [MenuItem("Fangtang/Level/Select 选择/Select All on Left 选择左边所有")]
    private static void DoSelectAllOnLeft()
    {
        Transform lastSelection = Selection.activeTransform;
        if (lastSelection == null)
        {
            return;
        }

        Selection.activeGameObject = null;

        List<UnityEngine.Object> selected = new List<UnityEngine.Object>();
        LevelComponent[] configs = GameObject.FindObjectsOfType(typeof(LevelComponent)) as LevelComponent[];
        foreach (LevelComponent config in configs)
        {
            if (config != null && 
                config.transform.position.x < lastSelection.position.x)
            {
                selected.Add(config.gameObject);
            }
        }
        Selection.objects = selected.ToArray() as UnityEngine.Object[];
    }

    [MenuItem("Fangtang/Level/Select 选择/Select All Between Objects 选择之间的所有", true)]
    private static bool ValidateDoSelectAllBetweenObjects()
    {
        return Selection.transforms.Length == 2;
    }

    [MenuItem("Fangtang/Level/Select 选择/Select All Between Objects 选择之间的所有")]
    private static void DoSelectAllBetweenObjects()
    {
        if (Selection.transforms.Length != 2)
        {
            Debug.LogError("You must choose 2 objects.");
            return;
        }

        Transform leftSelection;
        Transform rightSelection;
        if (Selection.transforms[0].position.x < Selection.transforms[1].position.x)
        {
            leftSelection = Selection.transforms[0];
            rightSelection = Selection.transforms[1];
        }
        else
        {
            leftSelection = Selection.transforms[1];
            rightSelection = Selection.transforms[0];
        }

        Selection.activeGameObject = null;

        List<UnityEngine.Object> selected = new List<UnityEngine.Object>();
        LevelComponent[] components = GameObject.FindObjectsOfType(typeof(LevelComponent)) as LevelComponent[];
        foreach (LevelComponent component in components)
        {
            if (component != null &&
                component.transform.position.x > leftSelection.position.x &&
                component.transform.position.x < rightSelection.position.x)
            {
                selected.Add(component.gameObject);
            }
        }
        Selection.objects = selected.ToArray() as UnityEngine.Object[];
    }

    [MenuItem("Fangtang/Level/Spawn Point/Reset to Origin")]
    private static void DoResetSpawnPoint()
    {
        PositionMarkup spawnPoint = FindObjectOfType(typeof(PositionMarkup)) as PositionMarkup;
        if (spawnPoint != null)
        {
            spawnPoint.transform.position = Vector3.zero;
        }
    }
}