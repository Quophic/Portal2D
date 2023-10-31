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
    public override void UpdateStatus()
    {
        base.UpdateStatus();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.sortingLayerID = target.closestPortal.linkedPortal.MaskLayerID;
        }
    }
}
