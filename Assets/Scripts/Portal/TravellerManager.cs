using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellerManager : MonoBehaviour
{
    private PortalTraveller[] travellers;
    private void Awake()
    {
        travellers = FindObjectsOfType<PortalTraveller>();
        StartCoroutine(LateFixedUpdate());
    }

    private void FixedUpdate()
    {
        foreach(var traveller in travellers)
        {
            traveller.UpdatePortalLayer();
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
