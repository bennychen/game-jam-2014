using GameJam.LevelEditUtil;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelPanelWindow : EditorWindow
{
    private void OnEnable()
    {
        if (_levelNames == null) { _levelNames = new string[0]; }
        if (_levelModes == null) { _levelModes = new string[0]; }
        if (_levelPath == null) { _levelPath = ResourcePath.LevelPath; }
        if (_levels == null) { _levels = new List<Level>(); }
        LoadAllLevel(_levelPath);
        _currentUseLevelID = LevelKitUtils.FindLevelIdInSceneById(_levels);   //find the level in Scene
        if (_groupList == null)
        {
            _groupList = (LevelComponentGroupList)AssetDatabase.LoadAssetAtPath(
                ResourcePath.LevelComponentDirectoryPath + "GroupList.asset", typeof(LevelComponentGroupList));
            if (_groupList == null)
            {
                Debug.LogError("Load LevelComponentGroupList error");
            }
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("关卡控制面板", EditorStyles.boldLabel);

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("当前场景载入的关卡 Current Load Level");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(_currentUseLevelID, EditorStyles.textField);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        if (GUILayout.Button("浏览文件位置 Browse Path", GUILayout.Height(20)))
        {
            _levelPath = EditorUtility.OpenFolderPanel("关卡文件位置", ResourcePath.LevelPath, "");
            _levelPath = _levelPath.Substring(_levelPath.IndexOf("Assets"));
            Debug.Log(_levelPath);
        }
        _levelPath = EditorGUILayout.TextField("关卡文件位置 Level File Path", _levelPath);
        if (GUILayout.Button("刷新关卡信息 Update Level info", GUILayout.Height(20)))
        {
            LoadAllLevel(_levelPath);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("当前目录关卡 level " + "Count : " + _levelNames.Length, EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("名称", GUILayout.Width(100));
        EditorGUILayout.LabelField("模式", GUILayout.Width(100));
        EditorGUILayout.LabelField("地图大小", GUILayout.Width(100));

        GUILayout.EndHorizontal();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        GUILayout.BeginHorizontal();
        _selectedLevelIndex = GUILayout.SelectionGrid(_selectedLevelIndex, _levelNames, 1, EditorStyles.boldLabel, GUILayout.Width(100));
        _selectedLevelIndex = GUILayout.SelectionGrid(_selectedLevelIndex, _levelModes, 1, EditorStyles.boldLabel, GUILayout.Width(100));
        _selectedLevelIndex = GUILayout.SelectionGrid(_selectedLevelIndex, _levelArea, 1, EditorStyles.boldLabel, GUILayout.Width(100));
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("载入关卡"))
        {
            _levelRootObject = CreateLevelInScene(_levels[_selectedLevelIndex]);
        }

        if (GUILayout.Button("删除关卡"))
        {
            if (EditorUtility.DisplayDialog("删除关卡" + _levels[_selectedLevelIndex].ID + "?", "", "yes", "no"))
            {
                DeleteSelectionLevel();
            }
        }
        if (GUILayout.Button("选择关卡"))
        {
            Selection.activeObject = _levels[_selectedLevelIndex];
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("保存场景中的关卡", GUILayout.Height(30)) && CheckIsExist())
        {
            Selection.activeGameObject = null;
            List<UnityEngine.Object> selected = new List<UnityEngine.Object>();
            Level saveLevel;
            if (_levelRootObject == null)
            {
                _levelRootObject = GameObject.Find("Level[" + _currentUseLevelID + "]");
            }
            saveLevel = LevelKitUtils.FindLevelById(_levels, _currentUseLevelID);
            if (saveLevel == null)
            {
                Debug.LogError("关卡ID在场景中不存在");
            }
            LevelComponent[] components = GameObject.FindObjectsOfType(typeof(LevelComponent)) as LevelComponent[];
            GameObject[] objects = new GameObject[components.Length];
            for (int i = 0; i < components.Length; i++)
            {
                selected.Add(components[i].gameObject);
                objects[i] = components[i].gameObject;
            }
            Selection.objects = selected.ToArray() as UnityEngine.Object[];

            LevelCreateWindow saveWindow = EditorWindow.GetWindow<LevelCreateWindow>();
            saveWindow.InitializeSave(objects, saveLevel);
        }
        if (GUILayout.Button("清除场景中的关卡组件", GUILayout.Height(30)))
        {
            LevelEditorMenu.DoSelectAll();
            LevelEditorMenu.DoDeleteAll();
        }

        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("创建组件（选中）", GUILayout.Height(30)))
        {
            if (Selection.gameObjects.Length > 0)
            {
                LevelComponentCreateWindow window = (LevelComponentCreateWindow)EditorWindow.GetWindow(typeof(LevelComponentCreateWindow));
                window.Initialize(Selection.gameObjects);
            }
            else
            {
                Debug.LogError("Please choose some game objects first.");
            }
        }
        EditorGUILayout.Separator();
        if (GUILayout.Button("创建LevelComponentGroup", GUILayout.Height(30)))
        {
            CustomAssetUtility.CreateAsset<LevelComponentGroup>();
        }
        EditorGUILayout.Separator();
        if (GUILayout.Button("创建一个关卡", GUILayout.Height(30)))
        {
            LevelCreateWindow window = EditorWindow.GetWindow<LevelCreateWindow>();
            window.Initialize(Selection.gameObjects);
        }
        EditorGUILayout.Separator();
        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();
    }

    private bool CheckIsExist()
    {
        _levelRootObject = GameObject.Find("Level[" + _currentUseLevelID + "]");
        if (_levelRootObject == null)
        {
            Debug.LogError("场景中没有找到你载入的关卡");
            return false;
        }
        Debug.Log("find " + _levelRootObject.name + "in scene");
        return true;
    }

    private GameObject CreateLevelInScene(Level level)
    {
        string levelname = level.ID;
        string progressBarTitle = "Loading level [" + levelname + "]";
        EditorUtility.DisplayProgressBar(progressBarTitle, "Preparing...", 0);

        GameObject levelRoot = new GameObject("Level[" + levelname + "]");

        Selection.activeGameObject = null;

        List<UnityEngine.Object> selected = new List<UnityEngine.Object>();
        float progress = 0;
        foreach (var subcomponent in level.Elements)
        {
            progress += 1f / level.Elements.Count;
            EditorUtility.DisplayProgressBar(progressBarTitle, "Loading [" + subcomponent.ComponentID + "]", progress);

            LevelComponent levelComponentPrefab;
            Debug.Log(subcomponent.ComponentGroup.name);
            if (subcomponent.ComponentGroup.TryGetLevelComponent(subcomponent.ComponentID, out levelComponentPrefab))
            {
                GameObject gameObject = PrefabUtility.InstantiatePrefab(levelComponentPrefab.gameObject) as GameObject;
                gameObject.transform.position = subcomponent.Position;
                gameObject.transform.rotation = subcomponent.Rotation;
                gameObject.transform.localScale = subcomponent.Scale;
                gameObject.transform.parent = levelRoot.transform;
                selected.Add(gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(levelRoot);
                Debug.LogError("Couldn't find level component [" + subcomponent.ComponentID + "] from groupList[" + subcomponent.ComponentGroup.name+"]");
                EditorUtility.ClearProgressBar();
                return null;
            }
        }

        Selection.objects = selected.ToArray() as UnityEngine.Object[];

        EditorUtility.ClearProgressBar();
        _currentUseLevelID = level.ID;
        return levelRoot;
    }

    private void DeleteSelectionLevel()
    {
        if (AssetDatabase.DeleteAsset(_levelPath + "/" + _levels[_selectedLevelIndex].name + ".asset"))
        {
            LoadAllLevel();
        }
    }
    private void LoadAllLevel()
    {
        LoadAllLevel(_levelPath);
    }

    private void LoadAllLevel(string _levelPath)
    {
        if (string.IsNullOrEmpty(_levelPath))
        {
            Debug.LogError("路径为空！");
        }
        string[] fileEntries = System.IO.Directory.GetFiles(_levelPath, "*.asset");
        if (fileEntries.Length <= 0)
        {
            Debug.LogError("你选择的路径没有关卡文件存在！");
        }
        _levels.Clear();
        _levelNames = new string[fileEntries.Length];
        _levelModes = new string[fileEntries.Length];
        _levelArea = new string[fileEntries.Length];
        for (int i = 0; i < fileEntries.Length; i++)
        {
            Level level = (Level)AssetDatabase.LoadAssetAtPath(fileEntries[i], typeof(Level));
            if (level != null)
            {
                _levels.Add(level);
                _levelNames[i] = level.ID;
                _levelModes[i] = level.Mode.ToString();
            }
            else
            {
                Debug.LogError("加载关卡资源失败");
            }
        }
    }

    private LevelComponentGroupList _groupList;
    private GameObject _levelRootObject;
    private List<Level> _levels;
    private string _levelPath;
    private string _currentUseLevelID;
    private Vector2 _scrollPosition;

    private int _selectedLevelIndex;
    private static string[] _levelNames = new string[0];
    private static string[] _levelModes = new string[0];
    private static string[] _levelArea = new string[0];

}
