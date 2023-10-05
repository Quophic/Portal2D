using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Portal portalPrefab;
    public Transform playerEye;
    public Camera playerCamera;
    public Portal portalRed;
    public Portal portalBlue;

    public bool connected => portalRed != null && portalRed.gameObject.activeInHierarchy && portalBlue != null && portalBlue.gameObject.activeInHierarchy; 

    void Start()
    {
        portalRed = Instantiate(portalPrefab);
        portalBlue = Instantiate(portalPrefab);
        portalRed.gameObject.SetActive(false);
        portalBlue.gameObject.SetActive(false);

        portalRed.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        portalBlue.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        portalRed.linkedPortal = portalBlue;
        portalBlue.linkedPortal = portalRed;
        portalRed.playerEye = playerEye;
        portalBlue.playerEye = playerEye;

        portalRed.playerCamera = playerCamera;
        portalBlue.playerCamera = playerCamera;
        portalRed.localLayer = LayerMask.NameToLayer("NearPortalRed");
        portalBlue.localLayer = LayerMask.NameToLayer("NearPortalBlue");
    }

    private void Update()
    {
    }

    public void SetPortalRed(Vector3 position, Quaternion rotation)
    {
        SetPortal(portalRed, position, rotation);
    }
    public void SetPortalBlue(Vector3 position, Quaternion rotation)
    {
        SetPortal(portalBlue, position, rotation);
    }

    private void SetPortal(Portal portal, Vector3 position, Quaternion rotation)
    {
        portal.gameObject.SetActive(true);
        portal.transform.SetLocalPositionAndRotation(position, rotation);
    } 
    

    public void SetPortalCamera()
    {
        portalRed.SetCameraTransform();
        portalRed.Render();
        portalBlue.SetCameraTransform();
        portalBlue.Render();
    }
}
