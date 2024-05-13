using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PortalStaticSnap : MonoBehaviour
{
    public BoxCollider2D SnapZone;
    public PolygonCollider2D ColliderSnaps;
    public GameObject snap;

    List<Vector2> self => GetBoxCollider2DCorners(SnapZone);

    bool isClockWise => Vector3.Dot(transform.forward, Vector3.forward) >= 0;

    public void SetLayer(LayerMask layer)
    {
        snap.layer = layer;
    }

    public void GenerateSnap()
    {
        float zRotation = transform.rotation.eulerAngles.z;
        zRotation = isClockWise ? zRotation : -zRotation;
        var r = Physics2D.OverlapBoxAll(transform.position, SnapZone.size, zRotation, LayerMask.GetMask("Ground"));
        if (r != null)
        {
            ColliderSnaps.pathCount = 0;
            for (int i = 0; i < r.Length; i++)
            {
                Collider2D c = r[i];
                if (c is BoxCollider2D)
                {
                    List<Vector2> world = GetBoxCollider2DCorners(c as BoxCollider2D);
                    List<Vector2> result = PolygonUtils.Intersection(world, self);
                    TransformLocal(result);
                    if (result.Count != 0)
                    {
                        ColliderSnaps.SetPath(ColliderSnaps.pathCount++, result);
                    }
                }
                else if (c is CompositeCollider2D)
                {
                    var world = GetCompositeCollider2DCorners(c as CompositeCollider2D);
                    foreach (var points in world)
                    {
                        List<Vector2> result = PolygonUtils.Intersection(points, self);
                        TransformLocal(result);
                        if (result.Count != 0)
                        {
                            ColliderSnaps.SetPath(ColliderSnaps.pathCount++, result);
                        }
                    }
                }
            }
        }
    }

    private List<Vector2> GetBoxCollider2DCorners(BoxCollider2D b)
    {
        List<Vector2> result = new List<Vector2>();
        Matrix4x4 m = b.gameObject.transform.localToWorldMatrix;
        result.Add(m.MultiplyPoint(new Vector2(-b.size.x, b.size.y) / 2));
        result.Add(m.MultiplyPoint(new Vector2(b.size.x, b.size.y) / 2));
        result.Add(m.MultiplyPoint(new Vector2(b.size.x, -b.size.y) / 2));
        result.Add(m.MultiplyPoint(new Vector2(-b.size.x, -b.size.y) / 2));
        if (!isClockWise)
        {
            for (int i = 0; i < result.Count / 2; i++)
            {
                Vector3 temp = result[i];
                result[i] = result[result.Count - 1 - i];
                result[result.Count - 1 - i] = temp;
            }
        }
        return result;
    }

    private List<List<Vector2>> GetCompositeCollider2DCorners(CompositeCollider2D c)
    {
        List<List<Vector2>> result = new List<List<Vector2>>();
        Matrix4x4 m = c.gameObject.transform.localToWorldMatrix;
        for (int i = 0; i < c.pathCount; i++)
        {
            // 这里拿到的点是逆时针的
            List<Vector2> points = new List<Vector2>();
            c.GetPath(i, points);
            for (int k = 0; k < points.Count; k++)
            {
                points[k] = m.MultiplyPoint(points[k]);
            }
            result.Add(points);
        }
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
