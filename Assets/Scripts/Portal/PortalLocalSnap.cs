using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLocalSnap : MonoBehaviour
{
    public BoxCollider2D SnapZone;
    public PolygonCollider2D ColliderSnaps;

    List<Vector2> self => GetBoxCollider2DCorners(SnapZone);
    private void FixedUpdate()
    {
        var r = Physics2D.OverlapBoxAll((Vector2)transform.position, SnapZone.size, transform.rotation.eulerAngles.z, LayerMask.GetMask("Ground"));
        if (r != null)
        {
            ColliderSnaps.pathCount = r.Length;
            for (int i = 0; i < r.Length; i++)
            {
                Collider2D c = r[i];
                if (c is BoxCollider2D)
                {
                    List<Vector2> world = GetBoxCollider2DCorners(c as BoxCollider2D);
                    List<Vector2> result = PolygonUtils.Intersection(world, self);
                    TransformLocal(result);
                    ColliderSnaps.SetPath(i, result);
                }
            }
        }
    }


    public List<Vector2> GetBoxCollider2DCorners(BoxCollider2D b)
    {
        List<Vector2> result = new List<Vector2>();
        Matrix4x4 m = b.gameObject.transform.localToWorldMatrix;
        result.Add(m.MultiplyPoint(new Vector2(-b.size.x, b.size.y) / 2));
        result.Add(m.MultiplyPoint(new Vector2(b.size.x, b.size.y) / 2));
        result.Add(m.MultiplyPoint(new Vector2(b.size.x, -b.size.y) / 2));
        result.Add(m.MultiplyPoint(new Vector2(-b.size.x, -b.size.y) / 2));
        return result;
    }

    private void TransformLocal(List<Vector2> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = transform.worldToLocalMatrix.MultiplyPoint(points[i]);
        }
    }
}
