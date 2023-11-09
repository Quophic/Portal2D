using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBrain brain;
    public PortalTraveller playerTraveller;
    private Vector3 teleportedPos;
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
        teleportedPos = transform.position;
        teleportedPos.z = m.MultiplyPoint(transform.position).z;
        transform.position = teleportedPos;
        transform.rotation = m.rotation * transform.rotation;
    }
    public void SetCameraTransform()
    {
        if (playerTraveller.teleported)
        {

        }
    }
}
