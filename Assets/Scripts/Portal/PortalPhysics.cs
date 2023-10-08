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
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dis, layer);
        if (hit)
        {
            Debug.DrawLine(origin, origin + dir.normalized * dis, Color.white, 1f);
            return hit;
        }
        RaycastHit2D portalHit = ThroughPortal(origin, dir, dis);
        if (portalHit)
        {
            Portal portal = portalHit.collider.GetComponent<Portal>();
            Vector2 newOrigin = portal.TeleportMatrix.MultiplyPoint(portalHit.point);
            Vector2 newDir = portal.TeleportMatrix.MultiplyVector(dir);
            float newDis = dis - portalHit.distance;
            hit = Physics2D.Raycast(newOrigin, newDir, newDis, layer);

            Debug.DrawLine(origin, origin + dir.normalized * portalHit.distance, Color.white, 1f);
            Debug.DrawLine(newOrigin, newOrigin + newDir.normalized * newDis, Color.white, 1f);
        }
        return hit;
    }

}

