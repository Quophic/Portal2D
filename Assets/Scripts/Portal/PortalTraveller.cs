using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PortalTraveller : MonoBehaviour
{
    public PortalShadow shadowPrefab;
    public Vector2 lastPosition;
    public Vector2 CurrentPosition { get => transform.position; }
    public bool teleported;
    public Vector2 TeleportedPosition { get => rb2D.position; }
    public TravellerSpriteRenderer travellerRenderer;
    private LayerMask originLayer;
    private PortalShadow shadow;
    public float closestPortalSqrDis;
    public Portal closestPortal;

    public delegate void OnTeleport(Matrix4x4 teleportMatrix);
    public OnTeleport OnTeleported;
    private Rigidbody2D rb2D;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        originLayer = gameObject.layer;
    }

    private void LateUpdate()
    {
        UpdateShadow();
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
    

    public void Teleport(Matrix4x4 teleportMatrix)
    {
        teleported = true;
        rb2D.velocity = teleportMatrix.MultiplyVector(rb2D.velocity);
        rb2D.position = teleportMatrix.MultiplyPoint(rb2D.position);
        Vector3 rotation = teleportMatrix.rotation.eulerAngles;
        bool reversed = teleportMatrix.m22 < 0;
        rb2D.rotation = reversed ? -rotation.z : rotation.z;
        if(OnTeleported != null)
        {
            OnTeleported(teleportMatrix);
        }    
    }
    public void EnterPortalThreshold(Portal portal)
    {

    }
    public void ExitPortalThreshold(Portal portal)
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
