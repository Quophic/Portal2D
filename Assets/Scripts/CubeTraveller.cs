using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTraveller : PortalTraveller
{
    public Rigidbody2D rb2d;
    public override void Teleport(Matrix4x4 teleportMatrix)
    {
        rb2d.velocity = teleportMatrix.MultiplyVector(rb2d.velocity);
        transform.position = teleportMatrix.MultiplyPoint(transform.position);
        Quaternion rotation = teleportMatrix.rotation;
        transform.rotation = rotation * transform.rotation;
    }
}
