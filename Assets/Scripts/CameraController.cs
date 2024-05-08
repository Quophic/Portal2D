using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBrain brain;
    public PortalTraveller playerTraveller;
    public PlayerController playerController;
    public float freeMoveSpeed;
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

        if (Input.GetAxisRaw("FreeView") > 0)
        {
            playerController.enabled = false;
            virtualCamera.enabled = false;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Locked;

            var position = brain.transform.position;
            position += brain.transform.right * Input.GetAxisRaw("Mouse X") * freeMoveSpeed * Time.unscaledDeltaTime;
            position += brain.transform.up * Input.GetAxisRaw("Mouse Y") * freeMoveSpeed * Time.unscaledDeltaTime;
            brain.transform.position = position;
        }
        else
        {
            playerController.enabled = true;
            virtualCamera.enabled = true;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
        }

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
        brain.ManualUpdate();
    }
    public void RotateCamera()
    {
        t += Time.deltaTime * rotSpeed;
        float f = t;
        if (rotTime != 0)
        {
            f = t / rotTime;
        }
        transform.rotation = Quaternion.Slerp(oldRot, targetRot, curve.Evaluate(f));
    }
}
