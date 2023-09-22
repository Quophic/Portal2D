using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalTraveller : MonoBehaviour
{
    public abstract void Teleport(Matrix4x4 teleportMatrix);
    public abstract void EnterPortalThreshold();
    public abstract void ExitPortalThreshold();
}
