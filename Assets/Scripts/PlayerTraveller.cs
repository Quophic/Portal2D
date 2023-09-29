using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraveller : PortalTraveller
{
    public Rigidbody2D rb2d;
    public PortalController portalController;
    public CameraController CameraController;
    public PlayerController playerController;
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
        Quaternion rotation = teleportMatrix.rotation;
        transform.rotation = rotation * transform.rotation;

        CameraController.SetCameraTransform();
        portalController.SetPortalCamera();
    }
}
