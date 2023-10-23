using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalViewRenderer : MonoBehaviour
{
    public PortalController controller;
    public Shader portalViewShader;
    public int maxPortalIterateCount;
    private int iterateCount;
    private Material material;
    public Material Material
    {
        get
        {
            material = CheckShaderAndCreateMaterial(portalViewShader, material);
            return material;
        }
    }

    private float ratio => (float)Screen.width / Screen.height;

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
        if (controller.connected && Material)
        {
            float redDist = Vector3.Distance(controller.playerEye.position, controller.portalRed.transform.position);
            float blueDist = Vector3.Distance(controller.playerEye.position, controller.portalBlue.transform.position);
            
            RenderTexture tempTex = RenderTexture.GetTemporary(Screen.width, Screen.height);
            if(redDist > blueDist)
            {
                RenderView(controller.portalRed, source, tempTex);
                RenderView(controller.portalBlue, tempTex, destination);
            }
            else
            {
                RenderView(controller.portalBlue, source, tempTex);
                RenderView(controller.portalRed, tempTex, destination);
            }

            RenderTexture.ReleaseTemporary(tempTex);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void RenderView(Portal portal ,RenderTexture source, RenderTexture destination)
    {
        Vector3 portalToEye = controller.playerEye.position - portal.transform.position;
        float dot = Vector3.Dot(portalToEye, portal.transform.right);
        if(dot > 0)
        {
            RenderTexture portalView = RenderTexture.GetTemporary(Screen.width, Screen.height);
            StartRenderPortalView(portal, portalView);

            SetParamAndRender(Matrix4x4.identity, portal.Top, portal.Bottom, controller.playerEye.position, portalView, source, destination);
            RenderTexture.ReleaseTemporary(portalView);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void StartRenderPortalView(Portal portal, RenderTexture destination)
    {
        Graphics.Blit(portal.ViewTexture, destination);
        if (!portal.CanBeSeeThroughLinkedPortal)
        {
            return;
        }
        iterateCount = 0;
        Vector3 topPos = portal.linkedPortal.TeleportMatrix.MultiplyPoint(portal.Top);
        Vector3 bottomPos = portal.linkedPortal.TeleportMatrix.MultiplyPoint(portal.Bottom);
        Vector3 eyePos = portal.linkedPortal.TeleportMatrix.MultiplyPoint(controller.playerEye.position);
        RenderTexture buffer = RenderTexture.GetTemporary(Screen.width, Screen.height);
        _Render(eyePos);
        RenderTexture.ReleaseTemporary(buffer);

        void _Render(Vector3 eyePos)
        {
            if (iterateCount >= maxPortalIterateCount)
            {
                return;
            }
            iterateCount++;

            eyePos = portal.TeleportMatrix.MultiplyPoint(eyePos);

            _Render(eyePos);

            SetParamAndRender(portal.linkedPortal.TeleportMatrix, topPos, bottomPos, eyePos, destination, portal.ViewTexture, buffer);
            Graphics.Blit(buffer, destination);
        }
    }

    private void SetParamAndRender(Matrix4x4 offsetMatrix, Vector3 topPos, Vector3 bottomPos, Vector3 eyePos ,RenderTexture addition, RenderTexture source, RenderTexture destination)
    {
        Material.SetTexture("_PortalTex", addition);
        Material.SetVector("_Plane1", GetViewPortPlane(topPos, eyePos, bottomPos));
        Material.SetVector("_Plane2", GetViewPortPlane(bottomPos, eyePos, topPos));
        Vector4 portalPlane = GetViewPortPlane(topPos, bottomPos, eyePos);
        portalPlane.z *= -1;
        portalPlane.w *= -1;
        Material.SetVector("_Plane3", portalPlane);

        Vector2 origin = controller.playerCamera.WorldToViewportPoint(controller.playerEye.position);
        Vector2 dest = controller.playerCamera.WorldToViewportPoint(offsetMatrix.MultiplyPoint(controller.playerEye.position));
        Vector2 offset = dest - origin;
        Material.SetVector("_Offset", new Vector4(origin.x, origin.y, offset.x, offset.y));

        Vector2 col0 = offsetMatrix.GetColumn(0);
        Vector2 col1 = offsetMatrix.GetColumn(1);
        col0.y *= ratio;
        col1.x *= 1 / ratio;
        bool isPortalCameraReversed = Vector3.Dot(Vector3.forward, offsetMatrix.MultiplyVector(transform.forward)) < 0;
        if (!isPortalCameraReversed)
        {
            col0.y = -col0.y;
            col1.x = -col1.x;
        }
        Material.SetVector("_TMcol0", col0);
        Material.SetVector("_TMcol1", col1);
        Graphics.Blit(source, destination, Material);
    }
}
