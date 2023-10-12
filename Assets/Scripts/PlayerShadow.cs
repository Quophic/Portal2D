using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : PortalShadow
{
    public SpriteRenderer[] spriteRenderers;
    public Transform eye;
    public Transform body;
    public Transform portalGunSocket;
    public Transform portalGun;
    private Transform targetPlayer;
    private Transform targetBody;
    private Transform targetPortalGun;
    public override void InitStatus(PortalTraveller traveller)
    {
        targetPlayer = traveller.transform;
        targetBody = targetPlayer.Find("Body");
        targetPortalGun = targetBody.Find("PortalGunSocket").Find("PortalGun");
        foreach(SpriteRenderer sprite in spriteRenderers)
        {
            sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }
    public override void UpdateStatus(Portal portal)
    {
        Matrix4x4 m = portal.TeleportMatrix;
        transform.SetPositionAndRotation(m.MultiplyPoint(targetPlayer.position), m.rotation * targetPlayer.rotation);
        body.SetPositionAndRotation(m.MultiplyPoint(targetBody.position), m.rotation * targetBody.rotation);
        if (PortalPhysics.ThroughPortal(portalGunSocket.position, eye.position))
        {
            Matrix4x4 lm = portal.linkedPortal.TeleportMatrix;
            portalGun.SetPositionAndRotation(lm.MultiplyPoint(targetPortalGun.position), lm.rotation * targetPortalGun.rotation);
        }
        else
        {
            portalGun.SetPositionAndRotation(m.MultiplyPoint(targetPortalGun.position), m.rotation * targetPortalGun.rotation);
        }
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingLayerID = portal.linkedPortal.MaskLayerID;
        }
    }
}
