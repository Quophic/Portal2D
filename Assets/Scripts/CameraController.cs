using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBrain brain;
    public PortalTraveller playerTraveller;
    public float rotSpeed;
    private void Awake()
    {
        playerTraveller.OnTeleported += TeleportCamera;
    }
    void LateUpdate()
    {
        SetCameraTransform();
        
        brain.ManualUpdate();
    }

    public void TeleportCamera(Matrix4x4 m)
    {
        virtualCamera.ForceCameraPosition(m.MultiplyPoint(transform.position), m.rotation * transform.rotation);
    }
    public void SetCameraTransform()
    {
        Quaternion targetRot;
        if(Vector3.Dot(transform.forward, Vector3.forward) > 0)
        {
            targetRot = Quaternion.identity;
        }
        else
        {
            targetRot = Quaternion.Euler(0, 180, 0);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
    }
}
