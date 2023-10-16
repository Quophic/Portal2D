using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelection;
    public void ShowLevelSelection()
    {
        levelSelection.SetActive(true);
    }
}
