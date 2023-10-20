using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView : MonoBehaviour
{
    public LevelItem levelItemPrefab;
    public ScrollRect levelsView;

    private LevelManager manager;
    private RectTransform content;
    public void Start()
    {
        content = levelsView.content;
        manager = LevelManager.Instance;
        UpdateItem();
    }
    
    private void UpdateItem()
    {
        for(int i = 0; i < manager.Count; i++) 
        {
            LevelItem item = Instantiate(levelItemPrefab, content);
            item.LevelIndex = i;
        }
    }
    public void Back()
    {
        gameObject.SetActive(false);
    }
}
