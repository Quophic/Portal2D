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
    public Rigidbody2D Rb2D { get => rb2D; }
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        originLayer = gameObject.layer;
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
        Quaternion rotation = Quaternion.Euler(0, 0, rb2D.rotation);
        Quaternion newRotation = teleportMatrix.rotation * rotation;
        if (newRotation.eulerAngles.y < 90 && newRotation.eulerAngles.y > -90)
        {
            rb2D.rotation = newRotation.eulerAngles.z;
        }
        else
        {
            rb2D.rotation = -newRotation.eulerAngles.z;
            rb2D.angularVelocity = -rb2D.angularVelocity;
        }
        
        if (OnTeleported != null)
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
    public void UpdateShadow()
    {
        if(closestPortal == null)
        {
            DisableShadow();
        }
        else
        {
            EnableShadow();
            shadow.UpdateStatus(closestPortal);
        }
    }
}
