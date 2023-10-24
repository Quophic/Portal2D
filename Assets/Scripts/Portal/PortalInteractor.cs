using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteractor : MonoBehaviour
{
    public Camera portalCamera;
    public RenderTexture viewTexture;
    public LayerMask snapLayerMask;
    public LinkedSnap linkedSnap;
    
    private PortalStaticSnap[] staticSnaps;
    public PortalStaticSnap NeedLinkSnap
    {
        get
        {
            if(staticSnaps != null)
            {
                return staticSnaps[0];
            }
            else
            {
                return null;
            }
        }
    }

    public CloseToPortalChecker closeChecker;
    public SpriteMask spriteMask;

    public bool Actived
    {
        set => gameObject.SetActive(value);
    }
    public int MaskLayerID
    {
        get => spriteMask.frontSortingLayerID;
        set {
            spriteMask.frontSortingLayerID = value;
            spriteMask.backSortingLayerID = value;
        }
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
    }



    private void InitLocalSnap()
    {
        staticSnaps = GetComponentsInChildren<PortalStaticSnap>();
        foreach (var snap in staticSnaps)
        {
            snap.SetLayer(snapLayerMask);
        }
    }
    public void GenerateLocalSnap()
    {
        InitLocalSnap();
        foreach (PortalStaticSnap snap in staticSnaps)
        {
            snap.GenerateSnap();
        }
    }
    public void GenerateLinkedSnap(PortalStaticSnap needLinkedSnap, Matrix4x4 teleportMatrix)
    {
        linkedSnap.gameObject.layer = snapLayerMask;
        linkedSnap.GenerateLinkedSnap(needLinkedSnap, teleportMatrix);
    }

}
