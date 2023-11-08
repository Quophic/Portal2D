using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Gate : Effector
{
    public float speed;
    private Animation ani;
    private void Awake()
    {
        ani = GetComponent<Animation>();
        onActivated += OpenGate;
        onStopped += CloseGate;
    }
    private void OpenGate()
    {
        AnimationState state = ani[ani.clip.name];
        state.speed = speed;
        ani.Play();
    }
    private void CloseGate()
    {
        AnimationState state = ani[ani.clip.name];
        state.speed = -speed;
        if (!ani.isPlaying)
        {
            state.time = state.length;
        }
        ani.Play();
    }

}
