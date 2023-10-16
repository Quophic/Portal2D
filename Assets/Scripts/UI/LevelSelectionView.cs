using System.Collections;
using System.Collections.Generic;
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
        manager = new LevelManager();
        UpdateItem();
    }
    
    private void UpdateItem()
    {
        foreach(var info in manager.Infos) 
        {
            LevelItem item = Instantiate(levelItemPrefab, content);
            item.LevelName = info.levelName;
        }
    }
    public void Back()
    {
        gameObject.SetActive(false);
    }
}
