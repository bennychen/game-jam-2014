using System.Collections.Generic;
using UnityEngine;

public class LevelGround : ScriptableObject
{
    [SerializeField]
    public List<GameObject> GroundPrefabs;

    [SerializeField]
    public float GroundWidth;
}
