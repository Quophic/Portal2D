using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    // 控制生成的关卡之间的距离大小
    private readonly int LevelDistance = 1000;
    public static readonly string PATH = Application.streamingAssetsPath + "/Level/LevelInfos.json";
    private static LevelManager manager;
    private GameObject currentLevel = null;
    private GameObject previousLevel = null;
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
    private int currentLevelIndex;
    public int CurrentIndex => currentLevelIndex;
    public bool IsLastLevel => currentLevelIndex == levelInfos.Count - 1;
    public LevelInfo CurrentInfo
    {
        get => FindAt(currentLevelIndex);
    }
    public LevelInfo FindAt(int index)
    {
        if (index < 0 || index >= levelInfos.Count)
        {
            return new LevelInfo();
        }
        return levelInfos[index];
    }
    private List<LevelInfo> levelInfos = null;
    public LevelInfo[] Infos => levelInfos.ToArray();
    public int Count => levelInfos.Count;

    private LevelManager()
    {
        Load();
        currentLevelIndex = -1;
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
        if (levelInfos == null)
        {
            levelInfos = new List<LevelInfo>();
        }
    }
    public bool AddLevel(LevelInfo levelInfo)
    {
        foreach (var item in levelInfos)
        {
            if (item.path.Equals(levelInfo.path))
            {
                return false;
            }
        }
        levelInfos.Add(levelInfo);
        return true;
    }
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levelInfos.Count)
        {
            return;
        }
        if (previousLevel)
        {
            UnityEngine.Object.Destroy(previousLevel);
        }
        currentLevelIndex = index;
        previousLevel = currentLevel;
        var prefab = Resources.Load<GameObject>(CurrentInfo.path);
        currentLevel = UnityEngine.Object.Instantiate(prefab);
    }
    public bool LoadNextLevel()
    {
        if (IsLastLevel)
        {
            return false;
        }
        currentLevelIndex++;
        if (previousLevel)
        {
            UnityEngine.Object.Destroy(previousLevel);
        }
        previousLevel = currentLevel;
        var prefab = Resources.Load<GameObject>(CurrentInfo.path);
        currentLevel = UnityEngine.Object.Instantiate(prefab);
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
    public string path;

    public LevelInfo(string levelName, string path)
    {
        this.levelName = levelName;
        this.path = path;
    }
}
