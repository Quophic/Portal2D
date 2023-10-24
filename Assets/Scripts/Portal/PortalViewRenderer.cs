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

    private Vector4 GetViewFieldPlane(Vector3 origin, Vector3 onPlane, Vector3 onNormal)
    {
        Vector2 point = origin;
        Vector2 dir = onPlane;
        dir = (dir - point).normalized;
        Vector2 normal = onNormal;
        normal = (normal - point).normalized;
        normal = (normal - Vector2.Dot(dir, normal) * dir).normalized;
        return new Vector4(point.x, point.y, normal.x, normal.y);
    }

    private void SetPortalViewField(Camera portalCam, Vector3 portalTop, Vector3 portalBottom, Vector3 playerEye)
    {
        Vector2 top = portalCam.WorldToViewportPoint(portalTop);
        Vector2 bottom = portalCam.WorldToViewportPoint(portalBottom);
        Vector2 eye = portalCam.WorldToViewportPoint(playerEye);

        Material.SetVector("_Plane1", GetViewFieldPlane(top, eye, bottom));
        Material.SetVector("_Plane2", GetViewFieldPlane(bottom, eye, top));
        Vector4 portalPlane = GetViewFieldPlane(top, bottom, eye);
        portalPlane.z *= -1;
        portalPlane.w *= -1;
        Material.SetVector("_Plane3", portalPlane);
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

    private void SetAndRenderPortalView(Portal portal, Matrix4x4 offsetMatrix)
    {
        portal.SetPortalCamera(offsetMatrix);
        portal.Render();
    }

    private void RenderView(Portal portal ,RenderTexture source, RenderTexture destination)
    {
        Vector3 portalToEye = controller.playerEye.position - portal.transform.position;
        bool canSeeThroughPortal = Vector3.Dot(portalToEye, portal.transform.right) > 0;
        if (canSeeThroughPortal && maxPortalIterateCount > 0)
        {
            RenderTexture portalView = RenderTexture.GetTemporary(Screen.width, Screen.height);
            StartRenderPortalView(portal, portalView);

            SetParamAndRender(controller.playerCamera, portal, controller.playerEye.position, portalView, source, destination);
            RenderTexture.ReleaseTemporary(portalView);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void StartRenderPortalView(Portal portal, RenderTexture destination)
    {
        iterateCount = 0;
        RenderTexture buffer = RenderTexture.GetTemporary(Screen.width, Screen.height);
        _Render(Matrix4x4.identity);
        RenderTexture.ReleaseTemporary(buffer);

        void _Render(Matrix4x4 offsetMatrix)
        {
            iterateCount++;
            offsetMatrix *= portal.TeleportMatrix;
            if (iterateCount >= maxPortalIterateCount || !portal.CanBeSeeThroughLinkedPortal)
            {
                SetAndRenderPortalView(portal, offsetMatrix);
                Graphics.Blit(portal.ViewTexture, destination);
                return;
            }
            _Render(offsetMatrix);
            SetAndRenderPortalView(portal, offsetMatrix);
            SetParamAndRender(portal.PortalCamera, portal, offsetMatrix.MultiplyPoint(controller.playerEye.position), destination, portal.ViewTexture, buffer);
            Graphics.Blit(buffer, destination);
        }
    }

    private void SetParamAndRender(Camera camera, Portal portal, Vector3 eyePos ,RenderTexture addition, RenderTexture source, RenderTexture destination)
    {
        Material.SetTexture("_PortalTex", addition);
        SetPortalViewField(camera, portal.Top, portal.Bottom, eyePos);
        Graphics.Blit(source, destination, Material);
    }
}
