using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellerManager : MonoBehaviour
{
    private PortalTraveller[] travellers;
    private PortalController[] controllerList;
    private void Awake()
    {
        controllerList = FindObjectsOfType<PortalController>();
        travellers = FindObjectsOfType<PortalTraveller>();
        StartCoroutine(LateFixedUpdate());
    }

    private void FixedUpdate()
    {
        foreach (var traveller in travellers)
        {
            traveller.isTeleporting = false;
            traveller.lastPosition = traveller.CurrentPosition;

            traveller.UpdateShadow();
        }

    }

    IEnumerator LateFixedUpdate()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            foreach (var traveller in travellers)
            {
                traveller.ResetClosestPortal();
            }

            foreach (var controller in controllerList)
            {
                controller.UpdateTravellersClosestPortal();
                controller.CheckAndTeleportTravellers();
            }
            foreach (var traveller in travellers)
            {
                traveller.SetClosestPortalLayer();
            }
        }
    }
}
