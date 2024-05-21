using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelection;
    private void Awake()
    {
        LevelManager.Instance.LoadLevel(0);
        
    }
    public void ShowLevelSelection()
    {
        levelSelection.SetActive(true);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
