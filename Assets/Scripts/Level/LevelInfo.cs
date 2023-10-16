using System;
using UnityEngine;



[Serializable]
public struct LevelInfo
{
    public string levelName;
    public string scene;

    public LevelInfo(string levelName, string scene)
    {
        this.levelName = levelName;
        this.scene = scene;
    }
}
