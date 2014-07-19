using GameJam.LevelEditUtil;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelCreateWindow : EditorWindow
{
    public void Initialize(GameObject[] gameObjects)
    {
        BaseInit(gameObjects);
    }

    //when you want to save level
    public void InitializeSave(GameObject[] gameObjects, Level selectLevel)
    {
        BaseInit(gameObjects);

        _levelToSave = selectLevel;
    }

    private void OnEnable()
    {
        if (_levelToSave == null)
        {
            _levelToSave = ScriptableObject.CreateInstance<Level>();
            _levelToSave.ReBuildData();
        }
    }

    private void BaseInit(GameObject[] selectObjects)
    {
        _levelToSave = ScriptableObject.CreateInstance<Level>();
        _levelToSave.ReBuildData();
        foreach (GameObject gameObject in selectObjects)
        {
            LevelComponent component = gameObject.GetComponent<LevelComponent>();
            if (component != null)
            {
                _levelToSave.Elements.Add(new LevelElement(
                    component.ID,
                    component.ComponentGroup,
                    gameObject.transform.position,
                    gameObject.transform.rotation,
                    gameObject.transform.localScale));
                if (!_levelToSave.ComponentGroups.Contains(component.ComponentGroup))
                {
                    _levelToSave.ComponentGroups.Add(component.ComponentGroup);
                }
            }
        }
        _levelToSave.Elements.Sort(new LevelElementHorizontalSorter());
        _createPath = ResourcePath.LevelPath;
    }

    private void OnGUI()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Level Name 关卡名称", EditorStyles.boldLabel);
        _levelToSave.ID = EditorGUILayout.TextField(_levelToSave.ID, GUILayout.Width(200));

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Level Difficulty step 关卡难度分级", EditorStyles.boldLabel);
        _levelToSave.DifficultyStep = (LevelDifficulty)EditorGUILayout.EnumPopup(_levelToSave.DifficultyStep, GUILayout.Width(200));

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Level Components 关卡组件", EditorStyles.boldLabel);
        if (_levelToSave.Elements != null)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(" 分组:", EditorStyles.boldLabel);
            List<string> groupNames = new List<string>();
            groupNames = FindLevelComponentGroupNames(_levelToSave.Elements);

            GUILayout.SelectionGrid(0, groupNames.ToArray(), 3, EditorStyles.textField);

            EditorGUILayout.LabelField("Count: " + _levelToSave.Elements.Count, EditorStyles.label);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int i = 0; i < _levelToSave.Elements.Count; i++)
            {
                LevelElement Subcomponent = _levelToSave.Elements[i];
                EditorGUILayout.LabelField((i + 1) + ": " + Subcomponent.ComponentID, EditorStyles.numberField);
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.LabelField("Count: 0", EditorStyles.label);
        }
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Level Save In Path 关卡存储路径", EditorStyles.boldLabel);

        _createPath = EditorGUILayout.TextField(_createPath);
        EditorGUILayout.Separator();
        if (GUILayout.Button("Create Level Asset 创建", GUILayout.Height(30)))
        {
            if (!LevelKitUtils.CheckIfExist(_createPath + "/" + _levelToSave.ID + ".asset") ||
            EditorUtility.DisplayDialog("覆盖吗？",
                        "已经存在一个 [" + _levelToSave.ID + "] 关卡, 覆盖?", "是", "否"))
            {
                CreateLevel(_createPath + "/" + _levelToSave.ID + ".asset");
            }
        }
        EditorGUILayout.Separator();
    }

    private List<string> FindLevelComponentGroupNames(List<LevelElement> Elements)
    {
        
        List<string> names = new List<string>();
        for (int i = 0; i < Elements.Count; i++)
        {
            if (!names.Contains(Elements[i].ComponentGroup.name))
            {
                names.Add(Elements[i].ComponentGroup.name);
            }
        }
        return names;
    }

    private void CreateLevel(string createPath)
    {
        if (AssetDatabase.Contains(_levelToSave))
        {
            AssetDatabase.SaveAssets();
        }
        else
        {
            CustomAssetUtility.CreateAssetFromInstance<Level>(_levelToSave, createPath);
        }
        Close();
    }
  
    private string _createPath;
    private Vector2 _scrollPosition;
    //-- level
    private Level _levelToSave;
    //---
}