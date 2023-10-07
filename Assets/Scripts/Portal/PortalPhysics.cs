using UnityEngine;


static class PortalPhysics
{
    public static bool ThroughPortal(Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        float dis = dir.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(start, dir, dis, LayerMask.GetMask("PortalPlane"));
        return hit;
    }

}

