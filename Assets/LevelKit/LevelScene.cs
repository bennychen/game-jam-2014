using System.Collections.Generic;
using UnityEngine;

public class LevelScene : ScriptableObject
{
    public LevelGround Ground
    {
        get { return _ground; }
        set { _ground = value; }
    }

    public GameObject ParallaxScenePrefab
    {
        get { return _parallaxScenePrefab; }
        set { _parallaxScenePrefab = value; }
    }

    public IEnumerable<LevelComponent> ComponentPrefabs { get { return _idToComponents.Values; } }

    public void OnEnable()
    {
        RefreshComponents();
    }

    public bool ContainsLevelComponent(string id)
    {
        return _idToComponents.ContainsKey(id);
    }

    public LevelComponent GetLevelComponentPrefabById(string id)
    {
        if (_idToComponents.ContainsKey(id))
        {
            return _idToComponents[id];
        }
        else
        {
            Debug.LogError("Couldn't find level component [" + id + "] from scene.");
            return null;
        }
    }

    public bool TryGetLevelComponent(string id, out LevelComponent levelComponent)
    {
        return _idToComponents.TryGetValue(id, out levelComponent);
    }

    private void GatherLevelComponents(LevelComponentGroup group)
    {
        foreach (var config in group.Group)
        {
            if (config != null)
            {
                if (_idToComponents.ContainsKey(config.ID))
                {
                    Debug.LogError("level component [" + config.ID +
                        "] in group [" + group.name + "] already exists in the group");
                }
                else
                {
                    //Debug.Log("Add comopnent [" + config.ID + "] to scene [" + name + "]");
                    _idToComponents.Add(config.ID, config);
                }
            }
        }
    }

    private void RefreshComponents()
    {
        if (_idToComponents == null)
        {
            _idToComponents = new Dictionary<string, LevelComponent>();
        }
        _idToComponents.Clear();

        if (_levelComponentGroups != null)
        {
            foreach (var group in _levelComponentGroups)
            {
                if (group != null)
                {
                    GatherLevelComponents(group);
                }
            }
        }
    }

    private Dictionary<string, LevelComponent> _idToComponents;

    [SerializeField]
    private GameObject _parallaxScenePrefab;

    [SerializeField]
    private List<LevelComponentGroup> _levelComponentGroups;

    [SerializeField]
    private LevelGround _ground;
}