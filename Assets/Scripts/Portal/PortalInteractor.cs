using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteractor : MonoBehaviour
{
    public Camera portalCamera;
    public RenderTexture viewTexture;

    private void CreateViewTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            portalCamera.targetTexture = viewTexture;
        }
    }
    public void SetCameraPosition(Matrix4x4 teleportMatrix, Vector3 playerCameraPos, Quaternion playerCameraRot)
    {
        portalCamera.transform.SetPositionAndRotation(teleportMatrix.MultiplyPoint(playerCameraPos), teleportMatrix.rotation * playerCameraRot);
    }
    public void RenderPortalView()
    {
        CreateViewTexture();
        portalCamera.enabled = false;
        portalCamera.Render();
        portalCamera.enabled = true;
    }
}
