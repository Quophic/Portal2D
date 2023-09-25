using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalView : MonoBehaviour
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

            RenderTexture.ReleaseTemporary(tempTex);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void RenderPortalView(Portal portal ,RenderTexture source, RenderTexture destination)
    {
        Vector3 portalToEye = controller.playerEye.position - portal.transform.position;
        float dot = Vector3.Dot(portalToEye, portal.transform.right);
        if(dot > 0)
        {
            Matrix4x4 offsetMatrix = Matrix4x4.identity;
            Vector3 topPos = portal.Top;
            Vector3 bottomPos = portal.Bottom;
            Vector3 eyePos = controller.playerEye.position;
            int width = source.width;
            int height = source.height;
            RenderTexture buffer0 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(portal.ViewTexture, buffer0);
            StartRender(portal, topPos, bottomPos, eyePos, width, height, ref buffer0);

            Material.SetTexture("_PortalTex", buffer0);
            Material.SetVector("_Plane1", GetViewPortPlane(topPos, controller.playerEye.position, bottomPos));
            Material.SetVector("_Plane2", GetViewPortPlane(bottomPos, controller.playerEye.position, topPos));
            Vector4 portalPlane = GetViewPortPlane(topPos, bottomPos, controller.playerEye.position);
            portalPlane.z *= -1;
            portalPlane.w *= -1;
            Material.SetVector("_Plane3", portalPlane);

            Vector2 origin = controller.playerCamera.WorldToViewportPoint(controller.playerEye.position);
            Vector2 dest = controller.playerCamera.WorldToViewportPoint(eyePos);
            Vector2 offset = dest - origin;
            Material.SetVector("_Offset", new Vector4(origin.x, origin.y, offset.x, offset.y));

            float ratio = (float)Screen.width / Screen.height;
            Material.SetFloat("_Rotation", -Mathf.Deg2Rad * offsetMatrix.rotation.eulerAngles.z);
            Material.SetFloat("_Ratio", ratio);

            Graphics.Blit(source, destination, Material);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void StartRender(Portal portal, Vector3 topPos, Vector3 bottomPos, Vector3 eyePos, int width, int height, ref RenderTexture buffer0)
    {
        iterateCount = 0;
        topPos = portal.linkedPortal.TeleportMatrix.MultiplyPoint(topPos);
        bottomPos = portal.linkedPortal.TeleportMatrix.MultiplyPoint(bottomPos);
        eyePos = portal.linkedPortal.TeleportMatrix.MultiplyPoint(eyePos);
        Render(portal, topPos, bottomPos, eyePos, width, height, ref buffer0);
    }

    private void Render(Portal portal, Vector3 topPos, Vector3 bottomPos, Vector3 eyePos, int width, int height, ref RenderTexture buffer0)
    {
        if(iterateCount >= maxPortalIterateCount)
        {
            return;
        }
        iterateCount++;

        eyePos = portal.TeleportMatrix.MultiplyPoint(eyePos);

        Render(portal, topPos, bottomPos, eyePos, width, height, ref buffer0);

        Material.SetTexture("_PortalTex", buffer0);
        Material.SetVector("_Plane1", GetViewPortPlane(topPos, eyePos, bottomPos));
        Material.SetVector("_Plane2", GetViewPortPlane(bottomPos, eyePos, topPos));
        Vector4 portalPlane = GetViewPortPlane(topPos, bottomPos, eyePos);
        portalPlane.z *= -1;
        portalPlane.w *= -1;
        Material.SetVector("_Plane3", portalPlane);

        Vector2 origin = controller.playerCamera.WorldToViewportPoint(controller.playerEye.position);
        Vector2 dest = controller.playerCamera.WorldToViewportPoint(portal.linkedPortal.TeleportMatrix.MultiplyPoint(controller.playerEye.position));
        Vector2 offset = dest - origin;
        Material.SetVector("_Offset", new Vector4(origin.x, origin.y, offset.x, offset.y));

        float ratio = (float)Screen.width / Screen.height;
        Material.SetFloat("_Rotation", -Mathf.Deg2Rad * portal.linkedPortal.TeleportMatrix.rotation.eulerAngles.z);
        Material.SetFloat("_Ratio", ratio);

        RenderTexture buffer1 = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(portal.ViewTexture, buffer1, Material);
        RenderTexture.ReleaseTemporary(buffer0);
        buffer0 = buffer1;
    }
}
