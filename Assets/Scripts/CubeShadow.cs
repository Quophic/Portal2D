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
    public override void UpdateStatus()
    {
        base.UpdateStatus();
        spriteRenderer.sortingLayerID = target.closestPortal.linkedPortal.MaskLayerID;
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
}
