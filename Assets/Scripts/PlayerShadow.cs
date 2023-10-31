using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : PortalShadow
{
    public SpriteRenderer[] spriteRenderers;
    
    public override void InitStatus(PortalTraveller traveller)
    {
        base.InitStatus(traveller);
        
        foreach(SpriteRenderer sprite in spriteRenderers)
        {
            sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }
    public override void UpdateStatus(Portal portal)
    {
        base.UpdateStatus(portal);
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingLayerID = portal.linkedPortal.MaskLayerID;
        }
    }
}
