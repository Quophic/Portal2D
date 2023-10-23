using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public PortalController controller;
    private Camera PlayerCamera
    {
        get => controller.playerCamera;
    }
    public Portal linkedPortal;
    public PortalInteractor interactor;
    public Transform edgeTop;
    public Transform edgeBottom;
    public LayerMask nearLayer;
    public LayerMask localLayer
    {
        get => interactor.snapLayerMask;
        set => interactor.snapLayerMask = value;
    }
    public int MaskLayerID
    {
        get => interactor.MaskLayerID;
        set => interactor.MaskLayerID = value;
    }
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
    public RenderTexture ViewTexture { get { return interactor.viewTexture; } }
    public bool CanBeSeeThroughLinkedPortal
    {
        get
        {
            bool cameraSee = Vector3.Dot(transform.right, interactor.portalCamera.transform.position - transform.position) >= 0;
            bool linkedPortalSee = Vector3.Dot(transform.right, linkedPortal.transform.position - transform.position) >= 0;
            return cameraSee & linkedPortalSee;
        }
    }

    private void Awake()
    {
        interactor.Actived = false;
        travellers = new List<PortalTraveller>();
        interactor.closeChecker.OnClose = OnTravellerEnter;
        interactor.closeChecker.OnAway = OnTravellerExit;
    }
    private void Update()
    {
        SetCameraTransform();
        Render();
        CheckAndTeleportTravellers();
    }

    public void GenerateLocalSnap()
    {
        interactor.GenerateLocalSnap();
    }

    public void GenerateLinkedSnap()
    {
        interactor.GenerateLinkedSnap(linkedPortal.interactor.NeedLinkSnap, linkedPortal.TeleportMatrix);
    }

    public void SetCameraTransform()
    {
        interactor.SetCameraPosition(TeleportMatrix, PlayerCamera.transform.position, PlayerCamera.transform.rotation);
    }

    public void Render()
    {
        interactor.RenderPortalView();
    }

    private void CheckAndTeleportTravellers()
    {
        foreach(PortalTraveller traveller in travellers)
        {
            if(PortalPhysics.ThroughPortal(traveller.lastPosition, traveller.CurrentPosition))
            {
                traveller.Teleport(TeleportMatrix);
                linkedPortal.OnTravellerEnter(traveller);
            }
            traveller.lastPosition = traveller.CurrentPosition;
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
}