using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour
{
    public List<Activator> activators;
    protected delegate void OnAction();
    protected OnAction onActivated;
    protected OnAction onStopped;
    public void OnActivated() {
        int count = 0;
        foreach(Activator a in activators)
        {
            if (a.Activated)
            {
                count++;
            }
        }
        if(count == activators.Count)
        {
            onActivated();
        }
    }
    public void OnStopped() => onStopped();
    private void Start()
    {
        SetActivators();   
    }
    [ContextMenu("SetActivators")]
    private void SetActivators()
    {
        foreach(Activator a in activators)
        {
            if (!a.effectors.Contains(this))
            {
                a.effectors.Add(this);
            }
        }
    }

}
