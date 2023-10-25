using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraveller : PortalTraveller
{
    public Rigidbody2D rb2d;
    public PortalController portalController;
    public CameraController CameraController;
    public PlayerController playerController;
    public override void EnterPortalThreshold(Portal portal)
    {
        base.EnterPortalThreshold(portal);
    }

    public override void ExitPortalThreshold(Portal portal)
    {
        base.ExitPortalThreshold(portal);
    }

    public override void Teleport(Matrix4x4 teleportMatrix)
    {
        base.Teleport(teleportMatrix);
        rb2d.velocity = teleportMatrix.MultiplyVector(rb2d.velocity);
        rb2d.position = teleportMatrix.MultiplyPoint(rb2d.position);
        transform.position = teleportMatrix.MultiplyPoint(transform.position);
        Quaternion rotation = teleportMatrix.rotation;
        transform.rotation = rotation * transform.rotation;
        CameraController.SetCameraTransform();
    }
}
