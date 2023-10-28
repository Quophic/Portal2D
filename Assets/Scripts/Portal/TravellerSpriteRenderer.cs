using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    private int[] originSortingLayerIDs;

    private void Awake()
    {
        originSortingLayerIDs = new int[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originSortingLayerIDs[i] = spriteRenderers[i].sortingLayerID;
            spriteRenderers[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
    }

    public void SetPortalLayer(Portal portal)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingLayerID = portal.MaskLayerID;
        }
    }

    public void RecoverSortingLayer()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingLayerID = originSortingLayerIDs[i];
        }
    }
}
