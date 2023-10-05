using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteractor : MonoBehaviour
{
    public Camera portalCamera;
    public RenderTexture viewTexture;
    public LayerMask snapLayerMask;
    public LinkedSnap linkedSnap;
    
    private PortalLocalSnap[] localSnaps;
    public PortalLocalSnap NeedLinkSnap
    {
        get
        {
            if(localSnaps != null)
            {
                return localSnaps[0];
            }
            else
            {
                return null;
            }
        }
    }

    public bool Actived
    {
        set => gameObject.SetActive(value);
    }

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



    private void InitLocalSnap()
    {
        localSnaps = GetComponentsInChildren<PortalLocalSnap>();
        foreach (var snap in localSnaps)
        {
            snap.SetLayer(snapLayerMask);
        }
    }
    public void GenerateLocalSnap()
    {
        InitLocalSnap();
        foreach (PortalLocalSnap snap in localSnaps)
        {
            snap.GenerateSnap();
        }
    }
    public void GenerateLinkedSnap(PortalLocalSnap needLinkedSnap, Matrix4x4 teleportMatrix)
    {
        linkedSnap.gameObject.layer = snapLayerMask;
        linkedSnap.GenerateLinkedSnap(needLinkedSnap, teleportMatrix);
    }

}
