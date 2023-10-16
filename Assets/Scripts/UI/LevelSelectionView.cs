using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView : MonoBehaviour
{
    public LevelItem levelItemPrefab;
    public ScrollRect levelsView;

    private RectTransform content;
    public void Start()
    {
        content = levelsView.content;
        AddItem();
    }
    
    private void AddItem()
    {
        LevelItem item = Instantiate(levelItemPrefab, content);
        item.LevelName = "new Level";
    }
    public void Back()
    {
        gameObject.SetActive(false);
    }
}
