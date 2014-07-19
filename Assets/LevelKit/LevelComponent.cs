using Fangtang.Utils;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{
    // OnBecomeActive happens in the same frame as OnEnable but after it and its transform is set to correct position
    public System.Action OnBecomeActive;

    [SerializeField]
    public string ID;
    [SerializeField]
    public float MinDepth = -3;
    [SerializeField]
    public float MaxDepth = 0.5f;
    [SerializeField]
    public bool IsScalable = false;

    [SerializeField]
    public bool SnapToVerticalLine = false;

    [SerializeField]
    public bool SnapToHorizontalLine = false;

    [SerializeField]
    public int PoolStartCount = 20;

    [SerializeField]
    public List<Collider> StaticColliders;

    [SerializeField]
    public bool IsHighEndOnly = false;

    [SerializeField]
    public LevelComponentGroup ComponentGroup;

#if UNITY_EDITOR
    public void DoSnapToGrid()
    {
        if (SnapToVerticalLine)
        {
            GridGizmos.Instance.SnapTransformToHorizontalGrid(transform);
        }
        if (SnapToHorizontalLine)
        {
            GridGizmos.Instance.SnapTransformToVerticalGrid(transform);
        }

        transform.SetPositionZ(ClampPositionZ(transform.position.z));

        if (!IsScalable)
        {
            transform.localScale = Vector3.one;
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
    }

    public float ClampPositionZ(float z)
    {
        return Mathf.Clamp(z, MinDepth, MaxDepth);
    }

    public void Rebuild()
    {
        UpdateStaticColliders();
    }

    public void UpdateStaticColliders()
    {
        if (StaticColliders == null)
        {
            StaticColliders = new List<Collider>();
        }
        StaticColliders.Clear();

        Collider[] colliders = GetComponentsInChildren<Collider>() as Collider[];
        foreach (var collider in colliders)
        {
            if (collider is BoxCollider)
            {
                BoxCollider boxCollider = collider as BoxCollider;
                if (boxCollider.size.z * boxCollider.transform.localScale.z < 20)
                {
                    Debug.LogWarning("Collider [" + boxCollider.name + "]'s z size is less than 20, rectify it");
                    boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 20);
                }
            }
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                StaticColliders.Add(collider);
            }
            else if (rigidbody.isKinematic == true)
            {
                Debug.Log("Destroy kinematic rigidbody of prefab [" + collider.name + "]");
                rigidbody.DestroyBasedOnRunning();
                StaticColliders.Add(collider);
            }
        }
    }

    public void AddRigidbodiesToStaticColliders()
    {
        foreach (var collider in StaticColliders)
        {
            Rigidbody rigidbody = collider.gameObject.GetComponentAndCreateIfNonExist<Rigidbody>();
            rigidbody.isKinematic = true;
        }
    }

#endif
}