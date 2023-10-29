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
        if(playerTraveller.teleported)
        {
            newPos = playerTraveller.closestPortal.linkedPortal.TeleportMatrix.MultiplyPoint(newPos);
        }
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
