using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    private int levelIndex;
    public int LevelIndex
    {
        get => levelIndex;
        set
        {
            levelIndex = value;
            var info = LevelManager.Instance.FindAt(levelIndex);
            text.text = info.levelName;
        }
    }

    private Button button;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadLevel);
    }
    private void LoadLevel()
    {
        LevelManager.Instance.LoadLevel(LevelIndex);
    }
}
