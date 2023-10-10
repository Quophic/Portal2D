using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalTraveller : MonoBehaviour
{
    public PortalShadow shadowPrefab;
    public SpriteRenderer[] spriteRenderers;
    public Vector2 lastPosition;
    public Vector2 CurrentPosition { get => checkPoint.transform.position; }
    public Transform checkPoint;
    private LayerMask originLayer;
    private List<Portal> portalsNear;
    private int[] originSortingLayerIDs;
    private PortalShadow shadow;
    private void Awake()
    {
        portalsNear = new List<Portal>();
        originLayer = gameObject.layer;
        originSortingLayerIDs = new int[spriteRenderers.Length];
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            originSortingLayerIDs[i] = spriteRenderers[i].sortingLayerID;
        }
    }
    private void FixedUpdate()
    {
        UpdatePortalLayer();
    }
    private void Update()
    {
        UpdateShadow();
    }

    private void UpdatePortalLayer()
    {
        if (portalsNear.Count == 0)
        {
            gameObject.layer = originLayer;
            for(int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sortingLayerID = originSortingLayerIDs[i];
                spriteRenderers[i].maskInteraction = SpriteMaskInteraction.None;
            }
        }
        else
        {
            Portal closestPortal = null;
            float closestSqrDis = float.MaxValue;
            foreach (Portal portal in portalsNear)
            {
                float sqrDis = Vector3.SqrMagnitude(portal.transform.position - transform.position);
                if (sqrDis < closestSqrDis)
                {
                    closestSqrDis = sqrDis;
                    closestPortal = portal;
                }
            }
            gameObject.layer = closestPortal.nearLayer;
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sortingLayerID = closestPortal.MaskLayerID;
                spriteRenderers[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            }
        }
    }

    public virtual void Teleport(Matrix4x4 teleportMatrix)
    {

    }
    public virtual void EnterPortalThreshold(Portal portal)
    {
        lastPosition = CurrentPosition;
        if (!portalsNear.Contains(portal))
        {
            portalsNear.Add(portal);
        }
        UpdatePortalLayer();
    }
    public virtual void ExitPortalThreshold(Portal portal)
    {
        if (portalsNear.Contains(portal))
        {
            portalsNear.Remove(portal);
        }
    }

    private void EnableShadow()
    {
        if(shadow == null)
        {
            shadow = Instantiate(shadowPrefab);
            shadow.InitStatus(this);
        }
        shadow.Enabled = true;
    }
    private void DisableShadow()
    {
        if(shadow == null)
        {
            return;
        }
        shadow.Enabled = false;
    }
    private void UpdateShadow()
    {
        Portal closestPortal = null;
        float closestSqrDis = float.MaxValue;
        foreach (Portal portal in portalsNear)
        {
            float sqrDis = Vector3.SqrMagnitude(portal.transform.position - transform.position);
            if (sqrDis < closestSqrDis)
            {
                closestSqrDis = sqrDis;
                closestPortal = portal;
            }
        }
        if(closestPortal == null)
        {
            DisableShadow();
        }
        else
        {
            EnableShadow();
            shadow.UpdateStatus(closestPortal);
            shadow.gameObject.layer = closestPortal.linkedPortal.localLayer;
        }
    }
}
