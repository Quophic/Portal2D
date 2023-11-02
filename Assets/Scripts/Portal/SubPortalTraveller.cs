using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPortalTraveller : MonoBehaviour
{
    public PortalTraveller parentTraveller;
    public Transform attached;
    public TravellerSpriteRenderer travellerRenderer;
    public Portal ClosestPortal;
    private Portal closestPortal;
    private Vector3 localPosition;
    private Quaternion localRotation;
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
            return;
        }
        Vector3 expectedPos = attached.localToWorldMatrix.MultiplyPoint(localPosition);
        Quaternion expectedRot = attached.rotation * localRotation;
        if (parentTraveller.closestPortal.CheckThrough(parentTraveller.TeleportedPosition, expectedPos))
        {
            expectedPos = parentTraveller.closestPortal.TeleportMatrix.MultiplyPoint(expectedPos);
            expectedRot = parentTraveller.closestPortal.TeleportMatrix.rotation * expectedRot;
            closestPortal = parentTraveller.closestPortal.linkedPortal;
        }
        else
        {
            closestPortal = parentTraveller.closestPortal;
        }
        transform.SetPositionAndRotation(expectedPos, expectedRot);
        travellerRenderer.SetPortalLayer(closestPortal);
    }
}
