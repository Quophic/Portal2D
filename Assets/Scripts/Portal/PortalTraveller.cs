using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalTraveller : MonoBehaviour
{
    public PortalShadow shadowPrefab;
    public Vector2 lastPosition;
    public Vector2 CurrentPosition { get => checkPoint.transform.position; }
    public Transform checkPoint;
    public TravellerSpriteRenderer travellerRenderer;
    private LayerMask originLayer;
    private PortalShadow shadow;
    public float closestPortalSqrDis;
    public Portal closestPortal;
    private void Awake()
    {
        originLayer = gameObject.layer;
    }

    private void LateUpdate()
    {
        UpdateShadow();
        lastPosition = CurrentPosition;
    }
    public void ResetClosestPortal()
    {
        closestPortal = null;
        closestPortalSqrDis = float.MaxValue;
    }
    public void SetClosestPortalLayer()
    {
        if(closestPortal == null)
        {
            gameObject.layer = originLayer;
            travellerRenderer.RecoverSortingLayer();
        }
        else{
            gameObject.layer = closestPortal.nearLayer;
            travellerRenderer.SetPortalLayer(closestPortal);
        }
    }
    

    public virtual void Teleport(Matrix4x4 teleportMatrix)
    {
        lastPosition = teleportMatrix.MultiplyPoint(lastPosition);
    }
    public virtual void EnterPortalThreshold(Portal portal)
    {

    }
    public virtual void ExitPortalThreshold(Portal portal)
    {
        
    }

    private void EnableShadow()
    {
        if(shadow == null)
        {
            shadow = Instantiate(shadowPrefab);
            shadow.InitStatus(this);
        }
        shadow.Enabled = true;
    }
    private void DisableShadow()
    {
        if(shadow == null)
        {
            return;
        }
        shadow.Enabled = false;
    }
    private void UpdateShadow()
    {
        if(closestPortal == null)
        {
            DisableShadow();
        }
        else
        {
            EnableShadow();
            shadow.UpdateStatus(closestPortal);
            shadow.gameObject.layer = closestPortal.linkedPortal.localLayer;
        }
    }
}
