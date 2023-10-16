using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
public class LevelTool
{
    public static readonly string PATH = Application.streamingAssetsPath + "/Level/LevelInfos.json";
    [MenuItem("Assets/Portal2D/Level/Add Current Level")]
    static void AddCurrentLevel()
    {
        if (!File.Exists(PATH))
        {
            using (File.Create(PATH)) { }
        }
        List<LevelInfo> infos = null;
        using (StreamReader sr = File.OpenText(PATH))
        {
            string readData = sr.ReadToEnd();
            infos = JsonConvert.DeserializeObject<List<LevelInfo>>(readData);
            
            sr.Close();
            sr.Dispose();
        }
        if (infos == null)
        {
            infos = new List<LevelInfo>();
        }
        string sceneName = SceneManager.GetActiveScene().name;
        infos.Add(new LevelInfo(sceneName, sceneName));
        using (StreamWriter sw = new StreamWriter(PATH))
        {
            string data = JsonConvert.SerializeObject(infos);
            sw.Write(data);
            sw.Close();
            sw.Dispose();
        }

        Debug.Log(" current open scene: " + SceneManager.GetActiveScene().path);
    }
}
