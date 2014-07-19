using System.Collections.Generic;
using UnityEngine;

public class LevelComponentGroup : ScriptableObject
{
    [SerializeField]
    public List<LevelComponent> Group;

    public void AddComponent(LevelComponent component) 
    {
        LevelComponent levelComponent;
        if (_idToLevelComponents.TryGetValue(component.ID, out levelComponent))
        {
            Debug.LogError("AddComponent has exist  add err");
        }
        Group.Add(component);
        RefreshDictionary();
    }

    public void ClearGroup()
    {
        Group.Clear();
        RefreshDictionary();
    }

    public void RefreshDictionary()
    {
        _idToLevelComponents.Clear();
        foreach (LevelComponent config in Group)
        {
            _idToLevelComponents.Add(config.ID, config);
        }
    }

    public bool TryGetLevelComponent(string id, out LevelComponent levelComponent)
    {
        if (_idToLevelComponents==null)
        {
            Debug.LogError("Dictionary is null");
        }
        return _idToLevelComponents.TryGetValue(id, out levelComponent);
    }

    protected void OnEnable()
    {
        if (Group == null)
        {
            Group = new List<LevelComponent>();
        }
        if (_idToLevelComponents == null)
        {
            _idToLevelComponents = new Dictionary<string, LevelComponent>();
        }
        RefreshDictionary();
    }

    private Dictionary<string, LevelComponent> _idToLevelComponents;
}
