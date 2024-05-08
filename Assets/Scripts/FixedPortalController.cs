using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPortalController : Effector
{
    public PortalController controller;
    private void Awake()
    {
        onActivated += controller.ActivatePortal;
        onStopped += controller.InactivatePortal;
    }
}
