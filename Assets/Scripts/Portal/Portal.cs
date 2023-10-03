using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public Transform playerEye;
    public GameObject attachedObj;
    public Collider2D effectZone;
    public Camera playerCamera;
    public Camera portalCamera;
    public Transform edgeTop;
    public Transform edgeBottom;
    public LayerMask localLayer;
    private List<PortalTraveller> travellers;
    public Vector3 Top { get => edgeTop.position; }
    public Vector3 Bottom { get => edgeBottom.position; }
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
    private RenderTexture viewTexture;
    public RenderTexture ViewTexture { get { return viewTexture; } }
    void CreateViewTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            portalCamera.targetTexture = viewTexture;
        }
    }
    private PortalLocalSnap[] snaps;
    private void Awake()
    {
        travellers = new List<PortalTraveller>();
        snaps = GetComponentsInChildren<PortalLocalSnap>();
    }
    private void Update()
    {
        CreateViewTexture();
        SetCameraTransform();
        CheckAndTeleportTravellers();
    }

    private void FixedUpdate()
    {
        foreach(PortalLocalSnap snap in snaps)
        {
            snap.SetLayer(localLayer);
            snap.GenerateSnap();
        }  
    }
    public void SetCameraTransform()
    {
        Matrix4x4 cameraMatrix = TeleportMatrix * playerCamera.transform.localToWorldMatrix;
        portalCamera.transform.SetPositionAndRotation(cameraMatrix.GetPosition(), cameraMatrix.rotation);
        portalCamera.enabled = false;
        portalCamera.Render();
        portalCamera.enabled = true;
    }

    private void CheckAndTeleportTravellers()
    {
        foreach(PortalTraveller traveller in travellers)
        {
            Vector3 pointToTraveller = traveller.checkPoint.transform.position - transform.position;
            if(Vector3.Dot(pointToTraveller, transform.right) < 0)
            {
                traveller.Teleport(TeleportMatrix);
            }
        }
        
    }
    private void OnTravellerEnter(PortalTraveller traveller)
    {
        if (travellers.Contains(traveller))
        {
            return;
        }
        travellers.Add(traveller);
        traveller.EnterPortalThreshold(this);
    } 
    private void OnTravellerExit(PortalTraveller traveller)
    {
        if (!travellers.Contains(traveller))
        {
            return;
        }
        travellers.Remove(traveller);
        traveller.ExitPortalThreshold(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalTraveller traveller = collision.gameObject.GetComponent<PortalTraveller>();
        if (traveller != null)
        {
            OnTravellerEnter(traveller);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PortalTraveller traveller = collision.gameObject.GetComponent<PortalTraveller>();
        if (traveller != null)
        {
            OnTravellerExit(traveller);
        }
    }
    
}