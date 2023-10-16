using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelItem : MonoBehaviour
{
    private LevelInfo info;
    public LevelInfo Info
    {
        get => info;
        set
        {
            info = value;
            text.text = value.levelName;
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
        SceneManager.LoadScene(info.scene);
    }
}
