using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Portal portalPrefab;
    public Camera playerCamera => Camera.main;
    public Portal portalRed;
    public Portal portalBlue;
    public Transform fixedPositionRed;
    public Transform fixedPositionBlue;

    public bool connected => portalRed != null && portalRed.gameObject.activeInHierarchy && portalBlue != null && portalBlue.gameObject.activeInHierarchy;

    private void Awake()
    {
        portalRed = Instantiate(portalPrefab);
        portalBlue = Instantiate(portalPrefab);
        portalRed.gameObject.SetActive(false);
        portalBlue.gameObject.SetActive(false);

        portalRed.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        portalBlue.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        portalRed.linkedPortal = portalBlue;
        portalBlue.linkedPortal = portalRed;

        portalRed.controller = this;
        portalBlue.controller = this;

        portalRed.nearLayer = LayerMask.NameToLayer("NearPortalRed");
        portalBlue.nearLayer = LayerMask.NameToLayer("NearPortalBlue");
        portalRed.localLayer = LayerMask.NameToLayer("PortalRed");
        portalBlue.localLayer = LayerMask.NameToLayer("PortalBlue");
        portalRed.MaskLayerID = SortingLayer.NameToID("NearPortalRed");
        portalBlue.MaskLayerID = SortingLayer.NameToID("NearPortalBlue");

        if (fixedPositionRed)
        {
            SetPortalRed(fixedPositionRed.position, fixedPositionRed.rotation);
        }
        if (fixedPositionBlue)
        {
            SetPortalBlue(fixedPositionBlue.position, fixedPositionBlue.rotation);
        }
        InactivatePortal();
    }

    public void ActivatePortal()
    {
        portalRed.gameObject.SetActive(true);
        portalBlue.gameObject.SetActive(true);
        OnPortalConnected();
    }
    public void InactivatePortal()
    {
        portalRed.gameObject.SetActive(false);
        portalBlue.gameObject.SetActive(false);
        OnPortalDisconnect();
    }

    public void UpdateTravellersClosestPortal()
    {
        portalRed.SetTravellerClosestPortal();
        portalBlue.SetTravellerClosestPortal();
    }

    public void CheckAndTeleportTravellers()
    {
        portalRed.CheckAndTeleportTravellers();
        portalBlue.CheckAndTeleportTravellers();
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
        if (connected)
        {
            OnPortalConnected();
        }
        else
        {
            OnPortalDisconnect();
        }
    }
    private void OnPortalConnected()
    {
        portalRed.interactor.Actived = true;
        portalBlue.interactor.Actived = true;

        portalRed.GenerateLocalSnap();
        portalBlue.GenerateLocalSnap();

        portalRed.GenerateLinkedSnap();
        portalBlue.GenerateLinkedSnap();
    }
    private void OnPortalDisconnect()
    {
        portalRed.interactor.Actived = false;
        portalBlue.interactor.Actived = false;
    }
}
