using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : Effector
{
    public float speed;
    private Transform top;
    private Transform bottom;
    private Vector2 openPos;
    private Vector2 closePos;
    private bool opening;
    private float t;
    private void Awake()
    {
        onActivated += CloseDoor;
        onStopped += OpenDoor;
        top = transform.Find("Top");
        bottom = transform.Find("Bottom");
        opening = true;
        closePos = top.localPosition;
        openPos = closePos + (Vector2)(top.localPosition - bottom.localPosition);
        t = 0;
    }
    private void FixedUpdate()
    {
        float n;
        if (opening)
        {
            n = t + speed * Time.fixedDeltaTime;
        }
        else
        {
            n = t - speed * Time.fixedDeltaTime;
        }
        t = Mathf.Clamp(n, 0, 1);
        top.localPosition = Vector2.Lerp(closePos, openPos, t);
        bottom.localPosition = -top.localPosition;
    }
    private void OpenDoor() => opening = true;
    private void CloseDoor() => opening = false;
}
