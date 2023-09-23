using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraveller : PortalTraveller
{
    public Rigidbody2D rb2d;
    public PortalController portalController;
    public CameraController CameraController;
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
        transform.position = teleportMatrix.MultiplyPoint(transform.position);
        CameraController.SetCameraTransform();
        portalController.SetPortalCamera();
    }
}
