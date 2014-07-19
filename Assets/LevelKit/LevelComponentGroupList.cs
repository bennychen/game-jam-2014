using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelComponentGroupList : ScriptableObject
{
    public static LevelComponentGroupList getInstance()
    {
        if (_instance == null) 
        {
            _instance = Resources.LoadAssetAtPath<LevelComponentGroupList>("Assets/LevelKit/Resources/LevelComponents/GroupList.asset");
        }
        return _instance;
    }

    [SerializeField]
    public List<LevelComponentGroup> GrouopList;

    public bool TryGetGroup(string groupName,out LevelComponentGroup group)
    {
        foreach (LevelComponentGroup item in GrouopList)
        {
            if (item.name.Equals(groupName))
            {
                group = item;
                return true;
            }
        }
        group = null;
        return false;
    }

    private void Awake()
    {
        if (GrouopList == null)
        {
            GrouopList = new List<LevelComponentGroup>();
        }
    }

    private static LevelComponentGroupList _instance;
}
