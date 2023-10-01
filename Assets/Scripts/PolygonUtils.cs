using System.Collections.Generic;
using UnityEngine;

static class PolygonUtils
{
    static public List<Vector2> Intersection(List<Vector2> polygon1, List<Vector2> polygon2)
    {
        List<Vector2> result = polygon1;
        for(int i = 0; i < polygon2.Count; i++)
        {
            int next = (i + 1) % polygon2.Count;
            Vector2 dir = polygon2[next] - polygon2[i];
            Vector2 normal = new Vector2(dir.y, -dir.x);
            result = Clip(result, polygon2[i], normal);
        }
        return result;
    }
    static private List<Vector2> Clip(List<Vector2> polygon, Vector2 point, Vector2 normal)
    {
        List<Vector2> result = new List<Vector2>();
        if( polygon == null || polygon.Count <= 3)
        {
            return result;
        }
        float pre = Vector2.Dot(polygon[polygon.Count - 1] - point, normal);
        for (int i = 0; i < polygon.Count; i++)
        {
            float cur = Vector2.Dot(polygon[i] - point, normal);
            if (cur < 0)
            {
                if(pre < 0)
                {
                }
                else
                {
                    int p = (i + polygon.Count - 1) % polygon.Count;
                    result.Add(ClipPoint(polygon[p], polygon[i], point, normal));
                }
            }
            else
            {
                if(pre < 0)
                {
                    int p = (i + polygon.Count - 1) % polygon.Count;
                    result.Add(ClipPoint(polygon[p], polygon[i], point, normal));
                    result.Add(polygon[i]);
                }
                else
                {
                    result.Add(polygon[i]);
                }
            }
            pre = cur;
        }
        return result;  
    }
    static private Vector2 ClipPoint(Vector2 pointA, Vector2 pointB, Vector2 point, Vector2 normal)
    {
        float a = Vector2.Dot(point - pointA, normal);
        float b = Vector2.Dot(pointB - pointA, normal);
        float t = a / b;
        return Vector2.Lerp(pointA, pointB, t);
    }
}

