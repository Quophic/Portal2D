using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public List<Effector> effectors;
    public bool Activated { get => activated; }
    private bool activated;
    public void Activate()
    {
        activated = true;
        foreach(var e in effectors)
        {
            e.OnActivated();
        }
    }
    public void Stop()
    {
        activated = false;
        foreach (var e in effectors)
        {
            e.OnStopped();
        }
    }
    private void Start()
    {
        SetEffectors();
    }
    [ContextMenu("SetEffectors")]
    private void SetEffectors()
    {
        foreach (Effector e in effectors)
        {
            if (!e.activators.Contains(this))
            {
                e.activators.Add(this);
            }
        }
    }
}
