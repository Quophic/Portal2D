using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraveller : PortalTraveller
{
    public Rigidbody2D rb2d;
    public override void EnterPortalThreshold()
    {
        base.EnterPortalThreshold();
    }

    public override void ExitPortalThreshold()
    {
        base.ExitPortalThreshold();
    }

    public override void Teleport(Matrix4x4 teleportMatrix)
    {
        rb2d.velocity = teleportMatrix.MultiplyVector(rb2d.velocity);
        rb2d.position = teleportMatrix.MultiplyPoint(rb2d.position);
    }
}
