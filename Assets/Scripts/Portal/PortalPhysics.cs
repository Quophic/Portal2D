using UnityEngine;


static class PortalPhysics
{
    public static bool ThroughPortal(Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        float dis = dir.magnitude;
        return ThroughPortal(start, dir, dis);
    }
    public static RaycastHit2D ThroughPortal(Vector2 origin, Vector2 dir, float dis)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dis, LayerMask.GetMask("PortalPlane"));
        return hit;
    }

    public static RaycastHit2D Raycast(Vector2 origin, Vector2 dir, float dis, LayerMask layer)
    {
        RaycastHit2D portalHit = ThroughPortal(origin, dir, dis);
        Portal portal = null;
        RaycastHit2D hit;
        if (portalHit)
        {
            portal = portalHit.collider.GetComponent<Portal>();
        }
        if (portal != null && portal.controller.connected)
        {
            hit = Physics2D.Raycast(origin, dir, portalHit.distance, layer);
            if (!hit)
            {
                Vector2 newOrigin = portal.TeleportMatrix.MultiplyPoint(portalHit.point);
                Vector2 newDir = portal.TeleportMatrix.MultiplyVector(dir);
                float newDis = dis - portalHit.distance;
                hit = Physics2D.Raycast(newOrigin, newDir, newDis, layer);
            }
        }
        else
        {
            hit = Physics2D.Raycast(origin, dir, dis, layer);
        }
        return hit;
    }

}

