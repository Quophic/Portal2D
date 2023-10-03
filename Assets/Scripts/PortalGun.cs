using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public PortalController portalController;
    
    void Update()
    {
        Aim();
    }
    private void Aim()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;
        transform.right = direction.normalized;
    }
    public void SetPortalRed()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.right, 100f, LayerMask.GetMask("Ground"));
        if (hit2D)
        {
            Matrix4x4 matrix = GetPortalMatrix(hit2D);
            portalController.SetPortalRed(matrix.GetPosition(), matrix.rotation);
        }
    }
    public void SetPortalBlue()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.right, 100f, LayerMask.GetMask("Ground"));
        if (hit2D)
        {
            Matrix4x4 matrix = GetPortalMatrix(hit2D);
            portalController.SetPortalBlue(matrix.GetPosition(), matrix.rotation);
        }
    }
    private Matrix4x4 GetPortalMatrix(RaycastHit2D hit2D)
    {
        Vector3 aimDirection = transform.right;
        Vector3 zDirection;
        if ( Vector3.Dot(hit2D.normal, Vector3.right) == 0)
        {
            zDirection = Vector3.Cross(hit2D.normal, aimDirection).normalized;
        }
        else
        {
            zDirection = Vector3.Cross(Vector3.up, aimDirection).normalized;
        }
        Vector3 xDirection = hit2D.normal;
        Vector3 yDirection = Vector3.Cross(zDirection, xDirection).normalized;
        Vector4 point = hit2D.point;
        point.w = 1;
        Matrix4x4 matrix = new Matrix4x4(xDirection, yDirection, zDirection, point);
        return matrix;
    }

}
