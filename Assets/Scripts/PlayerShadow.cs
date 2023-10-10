using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : PortalShadow
{
    public SpriteRenderer spriteRenderer;
    public Transform body;
    public Transform portalGun;
    private Transform targetPlayer;
    private Transform targetBody;
    private Transform targetPortalGun;
    public override void InitStatus(PortalTraveller traveller)
    {
        targetPlayer = traveller.transform;
        targetBody = targetPlayer.Find("Body");
        targetPortalGun = targetBody.Find("PortalGun");
    }
    public override void UpdateStatus(Portal portal)
    {
        Matrix4x4 m = portal.TeleportMatrix;
        transform.SetPositionAndRotation(m.MultiplyPoint(targetPlayer.position), m.rotation * targetPlayer.rotation);
        body.SetPositionAndRotation(m.MultiplyPoint(targetBody.position), m.rotation * targetBody.rotation);
        portalGun.SetPositionAndRotation(m.MultiplyPoint(targetPortalGun.position), m.rotation * targetPortalGun.rotation);
        spriteRenderer.sortingLayerID = portal.linkedPortal.MaskLayerID;
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
}
