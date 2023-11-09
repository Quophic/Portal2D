using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerEye;
    public float offsetFactor;
    public PortalTraveller playerTraveller;
    void Update()
    {
        SetCameraTransform();
    }

    public void SetCameraTransform()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = Vector2.Lerp(playerEye.position, mousePos, offsetFactor);
        Vector3 newRot = playerEye.rotation.eulerAngles;
        float reversed;
        if (playerTraveller.teleported)
        {
            Matrix4x4 m = playerTraveller.closestPortal.linkedPortal.TeleportMatrix;
            newPos = m.MultiplyPoint(newPos);
            reversed = Vector3.Dot(transform.forward, Vector3.forward);
            newRot.z += Mathf.Sign(reversed) * Mathf.Sign(m.m22) * m.rotation.eulerAngles.z;
        }
        reversed = Vector3.Dot(transform.forward, Vector3.forward);
        if (reversed < 0)
        {
            newPos.z = Mathf.Abs(transform.position.z);
        }
        else
        {
            newPos.z = -Mathf.Abs(transform.position.z);
        }
        
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(newRot);
    }
}
