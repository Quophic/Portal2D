using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalTraveller : MonoBehaviour
{
    public virtual void Teleport(Matrix4x4 teleportMatrix)
    {

    }
    public virtual void EnterPortalThreshold()
    {
        gameObject.layer = LayerMask.NameToLayer("NearPortal");
    }
    public virtual void ExitPortalThreshold()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
