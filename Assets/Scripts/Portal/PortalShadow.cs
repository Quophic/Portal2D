using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PortalShadow : MonoBehaviour
{
    protected PortalTraveller target;
    private Rigidbody2D rb2D;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    public bool Enabled
    {
        get => gameObject.activeInHierarchy;
        set => gameObject.SetActive(value);
    }
    public virtual void InitStatus(PortalTraveller traveller) 
    {
        target = traveller;
    }
    public virtual void UpdateStatus() 
    {
        gameObject.layer = target.closestPortal.linkedPortal.localLayer;
        rb2D.position = target.closestPortal.TeleportMatrix.MultiplyPoint(target.Rb2D.position);
        rb2D.velocity = target.closestPortal.TeleportMatrix.MultiplyVector(target.Rb2D.velocity);
        Quaternion targetRotation = Quaternion.Euler(0, 0, target.Rb2D.rotation);
        Quaternion newRotation = target.closestPortal.TeleportMatrix.rotation * targetRotation;
        if (newRotation.eulerAngles.y < 90 && newRotation.eulerAngles.y > -90)
        {
            rb2D.rotation = newRotation.eulerAngles.z;
            rb2D.angularVelocity = target.Rb2D.angularVelocity;
        }
        else
        {
            rb2D.rotation = -newRotation.eulerAngles.z;
            rb2D.angularVelocity = -target.Rb2D.angularVelocity;
        }
    }
}
