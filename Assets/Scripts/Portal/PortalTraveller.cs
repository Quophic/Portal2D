using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalTraveller : MonoBehaviour
{
    public Vector2 lastPosition;
    public Vector2 CurrentPosition { get => checkPoint.transform.position; }
    public Transform checkPoint;
    private LayerMask originLayer;
    private List<Portal> portalsNear;
    private void Awake()
    {
        portalsNear = new List<Portal>();
        originLayer = gameObject.layer;
    }
    private void FixedUpdate()
    {
        if(portalsNear.Count == 0)
        {
            gameObject.layer = originLayer;
        }
        else
        {
            Portal closestPortal = null;
            float closestSqrDis = float.MaxValue;
            foreach( Portal portal in portalsNear)
            {
                float sqrDis = Vector3.SqrMagnitude(portal.transform.position - transform.position);
                if (sqrDis < closestSqrDis)
                {
                    closestSqrDis = sqrDis;
                    closestPortal = portal;
                }
            }
            gameObject.layer = closestPortal.localLayer;
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
    }
    public virtual void ExitPortalThreshold(Portal portal)
    {
        if (portalsNear.Contains(portal))
        {
            portalsNear.Remove(portal);
        }
    }
}
