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
    public Camera PortalCamera => interactor.portalCamera;
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
    public void SetTravellerClosestPortal()
    {
        foreach(var traveller in travellers)
        {
            float sqrDis = Vector3.SqrMagnitude(transform.position - traveller.transform.position);
            if (sqrDis < traveller.closestPortalSqrDis)
            {
                traveller.closestPortalSqrDis = sqrDis;
                traveller.closestPortal = this;
            }
        }
    }
    public void GenerateLocalSnap()
    {
        interactor.GenerateLocalSnap();
    }

    public void GenerateLinkedSnap()
    {
        interactor.GenerateLinkedSnap(linkedPortal.interactor.NeedLinkSnap, linkedPortal.TeleportMatrix);
    }

    public void Render()
    {
        interactor.RenderPortalView();
    }

    public void SetPortalCamera(Matrix4x4 offsetMatrix) => interactor.SetCameraPosition(offsetMatrix, PlayerCamera.transform.position, PlayerCamera.transform.rotation); 

    public void CheckAndTeleportTravellers()
    {
        foreach(PortalTraveller traveller in travellers)
        {
            if(CheckThrough(traveller))
            {
                traveller.Teleport(TeleportMatrix);
                linkedPortal.OnTravellerEnter(traveller);
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
        if (traveller.teleported)
        {
            float sqrDis = Vector3.SqrMagnitude(transform.position - linkedPortal.TeleportMatrix.MultiplyPoint(traveller.transform.position));
            
            traveller.closestPortalSqrDis = sqrDis;
            traveller.closestPortal = this;
            traveller.SetClosestPortalLayer();
        }
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
    private bool CheckThrough(PortalTraveller traveller)
    {
        return CheckThrough(traveller.lastPosition, traveller.CurrentPosition);
    }
    public bool CheckThrough(Vector2 lastPos, Vector2 currentPos)
    {
        Vector3 lastToCurrent = currentPos - lastPos;
        Vector3 lastToTop = Top - (Vector3)lastPos;
        Vector3 lastToBottom = Bottom - (Vector3)lastPos;
        Vector3 crossCT = Vector3.Cross(lastToCurrent, lastToTop);
        Vector3 crossCB = Vector3.Cross(lastToCurrent, lastToBottom);
        bool lastAtFront = Vector3.Dot(transform.right, (Vector3)lastPos - transform.position) > 0;
        bool currentAtBehind = Vector3.Dot(transform.right, (Vector3)currentPos - transform.position) < 0;
        bool through = Vector3.Dot(crossCT, crossCB) < 0;
        return lastAtFront & currentAtBehind & through;
    }
}