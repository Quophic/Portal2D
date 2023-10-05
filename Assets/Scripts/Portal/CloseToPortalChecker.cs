using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseToPortalChecker : MonoBehaviour
{

    public delegate void OnPortalTraveller(PortalTraveller traveller);
    public OnPortalTraveller OnClose;
    public OnPortalTraveller OnAway;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalTraveller traveller = collision.gameObject.GetComponent<PortalTraveller>();
        if (traveller != null)
        {
            OnClose(traveller);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PortalTraveller traveller = collision.gameObject.GetComponent<PortalTraveller>();
        if (traveller != null)
        {
            OnAway(traveller);
        }
    }
}
