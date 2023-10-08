using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedSnap : MonoBehaviour
{
    public PolygonCollider2D linkedSnap;
    public void GenerateLinkedSnap(PortalStaticSnap needLinkedSnap, Matrix4x4 teleportMtr)
    {
        Matrix4x4 m = linkedSnap.transform.worldToLocalMatrix * teleportMtr * needLinkedSnap.transform.localToWorldMatrix;
        PolygonCollider2D snap = needLinkedSnap.ColliderSnaps;
        linkedSnap.pathCount = snap.pathCount;
        for(int i = 0; i < snap.pathCount; i++)
        {
            Vector2[] path = snap.GetPath(i);
            Vector2[] convertedPath = ConvertPath(m, path);
            linkedSnap.SetPath(i, convertedPath);
        }
    }

    private Vector2[] ConvertPath(Matrix4x4 m, Vector2[] path)
    {
        Vector2[] result = new Vector2[path.Length];
        for(int i = 0; i < path.Length; i++)
        {
            result[i] = m.MultiplyPoint(path[i]);
        }
        return result;
    }
}
