using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LevelItem : MonoBehaviour
{
    public string LevelName
    {
        get => text.text;
        set => text.text = value;
    }
    

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
