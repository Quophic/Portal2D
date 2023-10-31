using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShadow : PortalShadow
{
    public SpriteRenderer spriteRenderer;
    public override void InitStatus(PortalTraveller traveller)
    {
        base.InitStatus(traveller);
    }
    public override void UpdateStatus(Portal portal)
    {
        base.UpdateStatus(portal);
        spriteRenderer.sortingLayerID = portal.linkedPortal.MaskLayerID;
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
}
