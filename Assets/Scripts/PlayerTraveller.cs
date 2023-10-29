using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTraveller : PortalTraveller
{
    public Rigidbody2D rb2d;
    public PortalController portalController;
    public CameraController CameraController;
    public PlayerController playerController;
    public Transform playerEye;
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
        Vector3 rotation = teleportMatrix.rotation.eulerAngles;
        bool reversed = teleportMatrix.m22 < 0;
        rb2d.rotation = reversed ? -rotation.z : rotation.z;
        rotation.x = 0;
        rotation.z = 0;
        playerEye.localRotation = Quaternion.Euler(rotation) * playerEye.localRotation;
    }
}
