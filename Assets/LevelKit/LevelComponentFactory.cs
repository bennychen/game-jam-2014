using GameJam.Utils;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponentFactory : MonoBehaviour
{
    public void ReloadScene(LevelScene scene)
    {
        _currentScene = scene;
        RecreatePools();
    }

    public LevelComponent Create(string componentID)
    {
        return Create(componentID, Vector3.zero, Quaternion.identity, Vector3.one);
    }

    public LevelComponent Create(string componentID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (!_componentIdToPools.ContainsKey(componentID))
        {
            LevelComponent componentPrefab = _currentScene.GetLevelComponentPrefabById(componentID);
            if (componentPrefab != null)
            {
                AddPool(componentID, componentPrefab.gameObject, 1);
            }
            else
            {
                return null;
            }
        }

        LevelComponent component = GetNextFreeFromPool(componentID).GetComponent<LevelComponent>();
        component.transform.position = position;
        component.transform.rotation = rotation;
        component.transform.localScale = scale;

        if (component.OnBecomeActive != null)
        {
            component.OnBecomeActive();
        }

        return component;
    }

    private void RecreatePools()
    {
        DestroyAllObjects();

        foreach (var componentPrefab in _currentScene.ComponentPrefabs)
        {
            if (componentPrefab.PoolStartCount > 0)
            {
                AddPool(componentPrefab.ID,
                    componentPrefab.gameObject, componentPrefab.PoolStartCount);
            }
        }
    }

    private void AddPool(string levelComponentId, GameObject templatePrefab, int initialCapacity)
    {
        if (!_componentIdToPools.ContainsKey(levelComponentId))
        {
            GameObject subcontainer = new GameObject("Pool_" + levelComponentId);
            subcontainer.transform.parent = transform;

            ObjectRecycler recycler = new ObjectRecycler(templatePrefab, initialCapacity, subcontainer, (gameobject) =>
            {
#if UNITY_EDITOR
                gameobject.GetComponent<LevelComponent>().AddRigidbodiesToStaticColliders();
#endif
            });
            _componentIdToPools.Add(levelComponentId, recycler);
        }
    }

    private GameObject GetNextFreeFromPool(string levelComponentId)
    {
        if (_componentIdToPools.ContainsKey(levelComponentId))
        {
            return _componentIdToPools[levelComponentId].GetNextFree();
        }
        return null;
    }

    private void FreeAllPools()
    {
        foreach (var pool in _componentIdToPools.Values)
        {
            pool.FreeAllObjects();
        }
    }

    private void DestroyAllObjects()
    {
        foreach (var pool in _componentIdToPools.Values)
        {
            if (pool != null)
            {
                pool.DestroyAllObjects();
            }
        }
        _componentIdToPools.Clear();
    }

    private void Awake()
    {
        _componentIdToPools = new Dictionary<string, ObjectRecycler>();
    }

    private LevelScene _currentScene;
    private Dictionary<string, ObjectRecycler> _componentIdToPools;
}