using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public PortalInteractor interactor;
    public Transform playerEye;
    public Camera playerCamera;
    public Transform edgeTop;
    public Transform edgeBottom;
    public LayerMask localLayer
    {
        get => interactor.snapLayerMask;
        set => interactor.snapLayerMask = value;
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
    
    public CloseToPortalChecker closeChecker;

    private void Awake()
    {
        interactor.Actived = false;
        travellers = new List<PortalTraveller>();
        closeChecker.OnClose = OnTravellerEnter;
        closeChecker.OnAway = OnTravellerExit;
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
        interactor.SetCameraPosition(TeleportMatrix, playerCamera.transform.position, playerCamera.transform.rotation);
    }

    public void Render()
    {
        interactor.RenderPortalView();
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
}