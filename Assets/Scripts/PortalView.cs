using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalView : MonoBehaviour
{
    public PortalController controller;
    public Shader portalViewShader;
    private Material material;
    public Material Material
    {
        get
        {
            material = CheckShaderAndCreateMaterial(portalViewShader, material);
            return material;
        }
    }
    private Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null || !shader.isSupported)
        {
            return null;
        }
        if (material && material.shader == shader)
        {
            return material;
        }
        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        if (material)
        {
            return material;
        }
        else
        {
            return null;
        }
    }
    private Vector4 GetViewPortPlane(Vector3 origin, Vector3 onPlane, Vector3 onNormal)
    {
        Vector2 point = controller.playerCamera.WorldToViewportPoint(origin);
        Vector2 dir = controller.playerCamera.WorldToViewportPoint(onPlane);
        dir = (dir - point).normalized;
        Vector2 normal = controller.playerCamera.WorldToViewportPoint(onNormal);
        normal = (normal - point).normalized;
        normal = (normal - Vector2.Dot(dir, normal) * dir).normalized;
        return new Vector4(point.x, point.y, normal.x, normal.y);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Material)
        {
            float redDist = Vector3.Distance(controller.playerEye.position, controller.portalRed.transform.position);
            float blueDist = Vector3.Distance(controller.playerEye.position, controller.portalBlue.transform.position);
            
            RenderTexture tempTex = RenderTexture.GetTemporary(Screen.width, Screen.height);
            if(redDist > blueDist)
            {
                RenderPortalView(controller.portalRed, source, tempTex);
                RenderPortalView(controller.portalBlue, tempTex, destination);
            }
            else
            {
                RenderPortalView(controller.portalBlue, source, tempTex);
                RenderPortalView(controller.portalRed, tempTex, destination);
            }
            
            tempTex.Release();
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void RenderPortalView(Portal portal ,RenderTexture source, RenderTexture destination)
    {
        Material.SetTexture("_PortalTex", portal.ViewTexture);
        Material.SetVector("_Plane1", GetViewPortPlane(portal.Top, controller.playerEye.position, portal.Bottom));
        Material.SetVector("_Plane2", GetViewPortPlane(portal.Bottom, controller.playerEye.position, portal.Top));
        Vector4 portalPlane = GetViewPortPlane(portal.Top, portal.Bottom, controller.playerEye.position);
        portalPlane.z *= -1;
        portalPlane.w *= -1;
        Material.SetVector("_Plane3", portalPlane);
        Graphics.Blit(source, destination, Material);
    }
}
