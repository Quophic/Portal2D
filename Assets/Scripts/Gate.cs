using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Effector
{
    public float speed;
    private Transform page;
    private Vector2 openPos;
    private Vector2 closePos;
    private float t;
    private bool opening;
    private void Awake()
    {
        page = transform.Find("Page");
        onActivated += OpenGate;
        onStopped += CloseGate;
        opening = false;
        t = 0;
        closePos = page.localPosition;
        openPos.x = closePos.x;
        openPos.y = -closePos.y;
    }
    private void Update()
    {
        float n;
        if (opening)
        {
            n = t + speed * Time.deltaTime;
        }
        else
        {
            n = t - speed * Time.deltaTime;
        }
        t = Mathf.Clamp(n, 0, 1);
        page.localPosition = Vector2.Lerp(closePos, openPos, t);
    }
    private void OpenGate() => opening = true;
    private void CloseGate() => opening = false;
}
