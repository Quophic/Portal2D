using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellerManager : MonoBehaviour
{
    private PortalTraveller[] travellers;
    public PortalController controller;
    private void Awake()
    {
        travellers = FindObjectsOfType<PortalTraveller>();
        StartCoroutine(LateFixedUpdate());
    }

    private void FixedUpdate()
    {
        foreach (var traveller in travellers)
        {
            traveller.ResetClosestPortal();
        }
        controller.UpdateTravellersClosestPortal();
        foreach (var traveller in travellers)
        {
            traveller.SetClosestPortalLayer();
        }
    }

    IEnumerator LateFixedUpdate()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            foreach (var traveller in travellers)
            {
               
            }
        }
    }
}