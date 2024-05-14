using UnityEditor;

public class LevelTool
{
    [MenuItem("Assets/Portal2D/Add Current Level")]
    static void AddCurrentLevel()
    {
        // Ҫ���صĹؿ�Ԥ������Resource�ļ�����
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
