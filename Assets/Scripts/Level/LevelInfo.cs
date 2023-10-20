using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelManager
{
    public static readonly string PATH = Application.streamingAssetsPath + "/Level/LevelInfos.json";
    private static LevelManager manager;
    public static LevelManager Instance
    {
        get
        {
            if (manager == null)
            {
                manager = new LevelManager();
            }
            return manager;
        }
    }

    private List<LevelInfo> levelInfos = null;
    public LevelInfo[] Infos => levelInfos.ToArray();
    public int Count => levelInfos.Count;
    
    private LevelManager()
    {
        Load();
    }
    public void Load()
    {
        if (!File.Exists(PATH))
        {
            using (File.Create(PATH)) { }
        }
        using (StreamReader sr = File.OpenText(PATH))
        {
            string readData = sr.ReadToEnd();
            levelInfos = JsonConvert.DeserializeObject<List<LevelInfo>>(readData);

            sr.Close();
            sr.Dispose();
        }
        if(levelInfos == null)
        {
            levelInfos = new List<LevelInfo>();
        }
    }
    public bool AddLevel(LevelInfo levelInfo)
    {
        foreach(var item in levelInfos)
        {
            if (item.scene.Equals(levelInfo.scene))
            {
                return false;
            }
        }
        levelInfos.Add(levelInfo);
        return true;
    }
    public void Save()
    {
        using (StreamWriter sw = new StreamWriter(PATH))
        {
            string data = JsonConvert.SerializeObject(levelInfos);
            sw.Write(data);
            sw.Close();
            sw.Dispose();
        }
    }
}


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
