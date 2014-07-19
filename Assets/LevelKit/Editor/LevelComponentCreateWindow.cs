using GameJam.LevelEditUtil;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelComponentCreateWindow : EditorWindow
{
    public void Initialize(GameObject[] gameObjects)
    {
        if (gameObjects.Length > 0)
        {
            _gameObject = gameObjects[0]; //get first gameobject
        }
        initComponentGroup();
    }

    private void initComponentGroup()
    {
        _groupList = LevelComponentGroupList.getInstance();
        _groupLabels = new GUIContent[_groupList.GrouopList.Count];
        for (int i = 0; i < _groupList.GrouopList.Count; i++)
        {
            _groupLabels[i] = new GUIContent(_groupList.GrouopList[i].name);
            Debug.Log(_groupList.GrouopList[i].name);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.Separator();
        if (_gameObject != null)
        {
            _gameObject.name = EditorGUILayout.TextField("组件标识 Component ID", _gameObject.name, EditorStyles.numberField);
            _componentName = _gameObject.name;
        }
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("分组选择 Level Component Group", EditorStyles.boldLabel);
        _curSelectedGroupIndex = EditorGUILayout.Popup(_curSelectedGroupIndex, _groupLabels);
        _curSelecterGroup = _groupList.GrouopList[_curSelectedGroupIndex];


        EditorGUILayout.Separator();
        if (GUILayout.Button("Create Level Component Prefab", GUILayout.Height(30)))
        {
            if (_componentName.Length == 0)
            {
                Debug.LogError("Please give level component a new name.");
            }
            else if (_gameObject == null)
            {
                Debug.LogError("Please choose a game objects as subcomponents.");
            }
            else
            {
                if (!CheckIfExist() ||
                    EditorUtility.DisplayDialog("Replace?",
                        "There is already a level componnent called [" + _componentName + "], replace it?", "Yes", "No"))
                {
                    CreateLevelComponentPrefab();
                }
            }
            LevelComponentGroupEditor.UpdateGroup(_curSelecterGroup,ResourcePath.LevelComponentDirectoryPath + _curSelecterGroup.name + "/");
        }
        EditorGUILayout.Separator();
    }

    //此处负责创建ComponentPrefab  没有添加Prefab到 _curSelecterGroup
    private void CreateLevelComponentPrefab()
    {
        Selection.activeGameObject = null;

        List<UnityEngine.Object> selected = new List<UnityEngine.Object>();

        LevelComponent component;

        component = _gameObject.GetComponent<LevelComponent>();
        if (component == null)
        {
            component = _gameObject.AddComponent<LevelComponent>();
        }

        component.Rebuild();
        component.ID = _componentName;
        component.ComponentGroup = _curSelecterGroup;

        GameObject prefab = CreatePrefab(_gameObject.gameObject);
        Vector3 position = _gameObject.transform.position;
        GameObject.DestroyImmediate(_gameObject);
        GameObject.Instantiate(prefab, position, Quaternion.identity);
        selected.Add(prefab);
        Selection.objects = selected.ToArray() as UnityEngine.Object[];
        Close();
    }
    private GameObject CreatePrefab(GameObject levelComponentGameObject)
    {
        GameObject configPrefab = PrefabUtility.CreatePrefab(ResourcePath.LevelComponentDirectoryPath
            + _curSelecterGroup.name + "/" + levelComponentGameObject.name + ".prefab",
            levelComponentGameObject, ReplacePrefabOptions.ConnectToPrefab);
        configPrefab.transform.position = Vector3.zero;
        return configPrefab;
    }

    //检测是否存在同名的 component 在_curSelecterGroup 中
    private bool CheckIfExist()
    {
        LevelComponent component;
        if (_curSelecterGroup.TryGetLevelComponent(_gameObject.name, out component))
        {
            return true;
        }
        return false;
    }

    private LevelComponentGroupList _groupList;
    private GUIContent[] _groupLabels;
    private string _componentName;
    private int _curSelectedGroupIndex;
    private LevelComponentGroup _curSelecterGroup;

    private GameObject _gameObject;
}