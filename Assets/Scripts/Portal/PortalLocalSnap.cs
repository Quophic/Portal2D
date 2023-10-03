using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLocalSnap : MonoBehaviour
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

    private void TransformLocal(List<Vector2> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = transform.worldToLocalMatrix.MultiplyPoint(points[i]);
        }
    }
}
