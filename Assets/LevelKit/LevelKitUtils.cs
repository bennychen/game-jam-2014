using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class LevelKitUtils
{
    public static bool CheckIfExist(string path)
    {
        Debug.Log(path + " CheckIfExist");
        return System.IO.File.Exists(path);
    }

    public static Level FindLevelById(List<Level> levels, string id)
    {
        foreach (Level lvItem in levels)
        {
            if (lvItem.ID.Equals(id))
            {
                return lvItem;
            }
        }
        return null;
    }

    public static string FindLevelIdInSceneById(List<Level> levels)
    {
        foreach (Level lvItem in levels)
        {
            GameObject target = GameObject.Find("Level[" + lvItem.ID + "]");
            if (target)
            {
                return lvItem.ID;
            }
        }
        return null;
    }

    public static GameObject[] FindObjets(GameObject[] _objects, List<LevelElement> OutOfGroup)
    {
        GameObject[] targetObject = new GameObject[OutOfGroup.Count];
        int n = 0;
        for (int i = 0; i < _objects.Length; i++)
        {
            foreach(LevelElement item in OutOfGroup)
            {
                if (_objects[i].GetComponent<LevelComponent>().ID.Equals(item.ComponentID))
                {
                    targetObject[n++] = _objects[i];
                    Debug.Log(item.ComponentID);
                }
            }
        }
        return targetObject;
    }

    public static void CreatePrefebInPath(string path, GameObject[] objects)
    {
#if UNITY_EDITOR
        Debug.Log(path);
        
        for (int i = 0; i < objects.Length;i++)
        {
            Debug.Log(objects[i].name);
            GameObject configPrefab = PrefabUtility.CreatePrefab(path + objects[i].name + ".prefab",
                       objects[i], ReplacePrefabOptions.ConnectToPrefab);
            configPrefab.transform.position = Vector3.zero;
        }
#endif
    }
}
