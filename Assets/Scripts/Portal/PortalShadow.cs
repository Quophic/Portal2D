using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalShadow : MonoBehaviour
{
    public bool Enabled
    {
        get => gameObject.activeInHierarchy;
        set => gameObject.SetActive(value);
    }
    public virtual void InitStatus(PortalTraveller traveller) { }
    public virtual void UpdateStatus(Portal portal) { }
}
