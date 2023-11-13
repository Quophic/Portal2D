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
    private Quaternion targetRot;
    private Quaternion oldRot;
    private float t = 0;
    private float rotTime = 0;
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private void Awake()
    {
        playerTraveller.OnTeleported += TeleportCamera;
    }
    void Update()
    {
        RotateCamera();
        brain.ManualUpdate();
    }

    public void TeleportCamera(Matrix4x4 m)
    {
        virtualCamera.ForceCameraPosition(m.MultiplyPoint(transform.position), m.rotation * transform.rotation);
        oldRot = transform.rotation;
        if (Vector3.Dot(transform.forward, Vector3.forward) > 0)
        {
            targetRot = Quaternion.identity;
        }
        else
        {
            targetRot = Quaternion.Euler(0, 180, 0);
        }
        rotTime = Quaternion.Angle(transform.rotation, targetRot);
        t = 0;
    }
    public void RotateCamera()
    {
        t += Time.deltaTime * rotSpeed;
        float f = t;
        if(rotTime != 0)
        {
            f = t / rotTime;
        }
        transform.rotation = Quaternion.Slerp(oldRot, targetRot, curve.Evaluate(f));
    }
}
