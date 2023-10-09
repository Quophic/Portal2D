using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShadow : PortalShadow
{
    public SpriteRenderer spriteRenderer;
    private Transform targetCube;
    public override void InitStatus(PortalTraveller traveller)
    {
        targetCube = traveller.transform;
    }
    public override void UpdateStatus(Portal portal)
    {
        Matrix4x4 m = portal.TeleportMatrix;
        transform.SetPositionAndRotation(m.MultiplyPoint(targetCube.position), m.rotation * targetCube.rotation);
        spriteRenderer.sortingLayerID = portal.linkedPortal.MaskLayerID;
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
}
