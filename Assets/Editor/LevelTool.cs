using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelTool
{
    [MenuItem("Assets/Portal2D/Level/Add Current Level")]
    static void AddCurrentLevel()
    {
        LevelManager manager = LevelManager.Instance;
        string sceneName = SceneManager.GetActiveScene().name;
        manager.AddLevel(new LevelInfo(sceneName, sceneName));
        manager.Save();
    }
}
