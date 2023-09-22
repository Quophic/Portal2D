using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraveller : PortalTraveller
{
    public override void EnterPortalThreshold()
    {
        Debug.Log(name);
    }

    public override void ExitPortalThreshold()
    {
        
    }

    public override void Teleport(Matrix4x4 teleportMatrix)
    {
       
    }
}
