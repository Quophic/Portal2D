using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : Effector
{
    public float speed;
    public Menu menu;
    private Animation ani;
    private void Awake()
    {
        ani = GetComponent<Animation>();
        onActivated += delegate () { ani.Play(); };
    }
    private void LoadNextLevel() => menu.LoadNextLevel();
}
