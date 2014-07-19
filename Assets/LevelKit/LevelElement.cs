using UnityEngine;

[System.Serializable]
public class LevelElement
{
    public string ComponentID;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    [HideInInspector]
    public LevelComponentGroup ComponentGroup;

    public LevelElement()
    {
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
        Scale = Vector3.one;
    }

    public LevelElement(string id,
        LevelComponentGroup Group,
        Vector3 position,
        Quaternion rotation,
        Vector3 scale)
    {
        ComponentID = id;
        ComponentGroup = Group;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}
