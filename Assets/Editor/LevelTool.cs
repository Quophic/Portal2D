using UnityEditor;

public class LevelTool
{
    [MenuItem("Assets/Portal2D/Add Current Level")]
    static void AddCurrentLevel()
    {
        // 要加载的关卡预制体在Resource文件夹下
        if (Selection.objects.Length == 1)
        {
            LevelManager manager = LevelManager.Instance;
            manager.Load();
            var name = Selection.objects[0].name;
            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(Selection.objects[0]);
            if (path == null)
            {
                return;
            }
            path = path.Substring(17, path.Length - 24);
            manager.AddLevel(new LevelInfo(name, path));
            manager.Save();
        }
    }
}
