using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
public class LevelTool
{
    [MenuItem("Assets/Portal2D/Level/Add Current Level")]
    static void AddCurrentLevel()
    {
        LevelManager manager = new LevelManager();
        string sceneName = SceneManager.GetActiveScene().name;
        manager.AddLevel(new LevelInfo(sceneName, sceneName));
        manager.Save();
    }
}
