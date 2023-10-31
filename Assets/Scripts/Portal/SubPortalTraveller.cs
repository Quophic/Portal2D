using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPortalTraveller : MonoBehaviour
{
    public PortalTraveller parentTraveller;
    public Transform attached;
    private Vector3 localPosition;
    private Quaternion localRotation;
    private void Awake()
    {
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
    }
    public void CheckAndTelepot()
    {
        if(parentTraveller.closestPortal == null)
        {
            return;
        }
        Vector3 expectedPos = attached.localToWorldMatrix.MultiplyPoint(localPosition);
        Quaternion expectedRot = transform.rotation * localRotation;
        if (parentTraveller.closestPortal.CheckThrough(parentTraveller.TeleportedPosition, expectedPos))
        {
            expectedPos = parentTraveller.closestPortal.TeleportMatrix.MultiplyPoint(expectedPos);
            expectedRot = parentTraveller.closestPortal.TeleportMatrix.rotation * expectedRot;
        }
        transform.SetPositionAndRotation(expectedPos, expectedRot);
    }
}
