using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuObject;
    public GameObject pauseMenuObject;
    public TextMeshProUGUI levelNameText;
    public GameObject levelSelection;
    private void Awake()
    {
        LevelManager.Instance.LoadLevel(0);
        OpenMainMenu();
        ClosePauseMenu();
    }
    private void Update()
    {
        if (!mainMenuObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuObject.activeInHierarchy)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void OpenMainMenu()
    {
        mainMenuObject.SetActive(true);
    }
    public void CloseMainMenu()
    {
        mainMenuObject.SetActive(false);
    }
    public void ShowLevelSelection()
    {
        levelSelection.SetActive(true);
    }
    public void CloseLevelSelection()
    {
        levelSelection.SetActive(false);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenPauseMenu()
    {
        pauseMenuObject.SetActive(true);
        levelNameText.text = LevelManager.Instance.CurrentInfo.levelName;
    }
    public void ClosePauseMenu()
    {
        pauseMenuObject.SetActive(false);
    }
    public void Pause()
    {
        OpenPauseMenu();
        Time.timeScale = 0;
    }
    public void Resume()
    {
        ClosePauseMenu();
        Time.timeScale = 1;
    }
    //public void Restart()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //    Resume();
    //}
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Resume();
    }
}
