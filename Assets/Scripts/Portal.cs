using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public Transform playerEye;
    public Collider2D effectZone;
    public Camera playerCamera;
    public Camera portalCamera;
    private List<PortalTraveller> travellers;
    public Vector3 Top { get => transform.position + transform.up * transform.localScale.y / 2f; }
    public Vector3 Bottom { get => transform.position - transform.up * transform.localScale.y / 2f; }
    public Matrix4x4 TeleportMatrix { 
        get
        {
            if(linkedPortal != null)
            {
                return linkedPortal.transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0)) * transform.worldToLocalMatrix;
            }
            else
            {
                return Matrix4x4.identity;
            }
        } 
    }

    private void Awake()
    {
        travellers = new List<PortalTraveller>();
    }
    private void Update()
    {
        Matrix4x4 cameraMatrix = TeleportMatrix * playerCamera.transform.localToWorldMatrix;
        portalCamera.transform.SetPositionAndRotation(cameraMatrix.GetPosition(), cameraMatrix.rotation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalTraveller traveller = collision.gameObject.GetComponent<PortalTraveller>();
        if (traveller != null)
        {
            travellers.Add(traveller);
            Debug.Log("enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PortalTraveller traveller = collision.gameObject.GetComponent<PortalTraveller>();
        if (traveller != null)
        {
            travellers.Remove(traveller);
        }
    }
    
}