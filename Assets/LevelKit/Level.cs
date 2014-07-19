using System.Collections.Generic;
using UnityEngine;

public class Level : ScriptableObject
{
    //-----------------------------------------------gamekit   Common attribute
    [SerializeField]
    public string ID;                       //id 保持和文件名称一致，不要修改ID

    [SerializeField]
    public List<LevelElement> Elements;

    [SerializeField]
    public int Difficulty;

    [SerializeField]
    public LevelDifficulty DifficultyStep;

    [SerializeField]
    public LevelMode Mode;

    [SerializeField]
    [HideInInspector]
    public List<LevelComponentGroup> ComponentGroups;
    //-----------------------------------------------------------------------

    public void ReBuildData()
    {
        ID = "";
        Elements = new List<LevelElement>();
        Difficulty = 0;
        DifficultyStep = LevelDifficulty.Casual;
        Mode = LevelMode.normal;
        ComponentGroups = new List<LevelComponentGroup>();
    }
}
