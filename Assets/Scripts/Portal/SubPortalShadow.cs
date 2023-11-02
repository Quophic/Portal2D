using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPortalShadow : MonoBehaviour
{
    public TravellerSpriteRenderer travellerRenderer;
    private SubPortalTraveller target;
    public bool Enabled
    {
        get => gameObject.activeInHierarchy;
        set => gameObject.SetActive(value);
    }
    public void InitStatus(SubPortalTraveller subTraveller)
    {
        target = subTraveller;
    }
    public void UpdateStatus()
    {
        gameObject.layer = target.ClosestPortal.linkedPortal.localLayer;
        travellerRenderer.SetPortalLayer(target.ClosestPortal.linkedPortal);
        transform.position = target.ClosestPortal.TeleportMatrix.MultiplyPoint(target.transform.position);
        transform.rotation = target.ClosestPortal.TeleportMatrix.rotation * target.transform.rotation;
    }
}
