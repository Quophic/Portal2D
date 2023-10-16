using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject canvas;
    public TextMeshProUGUI levelNameText;
    private string levelName;
    private void Awake()
    {
        levelName = SceneManager.GetActiveScene().name;
        levelNameText.text = levelName;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas.activeInHierarchy)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        canvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        canvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {        
        SceneManager.LoadScene(levelName);
    }
}
