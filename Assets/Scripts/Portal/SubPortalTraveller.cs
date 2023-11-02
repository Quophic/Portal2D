using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPortalTraveller : MonoBehaviour
{
    public SubPortalShadow subShadowPrefab;
    public PortalTraveller parentTraveller;
    public Transform attached;
    public TravellerSpriteRenderer travellerRenderer;
    public Portal ClosestPortal { get => closestPortal; }
    private Portal closestPortal;
    private Vector3 localPosition;
    private Quaternion localRotation;
    private SubPortalShadow subShadow;
    private void Awake()
    {
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
    }
    public void CheckAndTeleport()
    {
        if(parentTraveller.closestPortal == null)
        {
            closestPortal = null;
            DisableShadow();
        }
        else
        {
            Vector3 expectedPos = attached.localToWorldMatrix.MultiplyPoint(localPosition);
            Quaternion expectedRot = attached.rotation * localRotation;
            if (parentTraveller.teleported)
            {
                closestPortal = parentTraveller.closestPortal;
                expectedPos = parentTraveller.closestPortal.linkedPortal.TeleportMatrix.MultiplyPoint(expectedPos);
                expectedRot = parentTraveller.closestPortal.linkedPortal.TeleportMatrix.rotation * expectedRot;
            }
            else if (parentTraveller.closestPortal.CheckThrough(parentTraveller.TeleportedPosition, expectedPos))
            {
                closestPortal = parentTraveller.closestPortal.linkedPortal;
                expectedPos = parentTraveller.closestPortal.TeleportMatrix.MultiplyPoint(expectedPos);
                expectedRot = parentTraveller.closestPortal.TeleportMatrix.rotation * expectedRot;
            }
            else
            {
                closestPortal = parentTraveller.closestPortal;
            }
            transform.SetPositionAndRotation(expectedPos, expectedRot);
            travellerRenderer.SetPortalLayer(closestPortal);
            EnableShadow();
            subShadow.UpdateStatus();
        }
    }

    private void EnableShadow()
    {
        if (subShadow == null)
        {
            subShadow = Instantiate(subShadowPrefab);
            subShadow.InitStatus(this);
        }
        subShadow.Enabled = true;
    }
    private void DisableShadow()
    {
        if (subShadow == null)
        {
            return;
        }
        subShadow.Enabled = false;
    }
}
