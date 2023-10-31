using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerEye;
    public float offsetFactor;
    public Camera mainCam;
    public PortalTraveller playerTraveller;
    void Update()
    {
        SetCameraTransform();
    }

    public void SetCameraTransform()
    {
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = Vector2.Lerp(playerEye.position, mousePos, offsetFactor);
        Vector3 newRot = playerEye.rotation.eulerAngles;
        if(playerTraveller.teleported)
        {
            Matrix4x4 m = playerTraveller.closestPortal.linkedPortal.TeleportMatrix;
            newPos = m.MultiplyPoint(newPos);
            float reversed = Vector3.Dot(transform.forward, Vector3.forward);
            newRot.z += Mathf.Sign(reversed) * Mathf.Sign(m.m22) * m.rotation.eulerAngles.z;
        }
        newPos.z = transform.position.z;
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(newRot);
    }
}
