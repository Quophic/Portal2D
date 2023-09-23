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

    void Start()
    {
        portalRed = Instantiate(portalPrefab);
        portalBlue = Instantiate(portalPrefab);
        portalRed.GetComponent<SpriteRenderer>().color = Color.red;
        portalBlue.GetComponent<SpriteRenderer>().color = Color.blue;
        portalRed.linkedPortal = portalBlue;
        portalBlue.linkedPortal = portalRed;
        portalRed.playerEye = playerEye;
        portalBlue.playerEye = playerEye;

        portalRed.playerCamera = playerCamera;
        portalBlue.playerCamera = playerCamera;
    }

    private void Update()
    {
        Ray redTopRay = new Ray(playerEye.position, portalRed.Top - playerEye.position);
        Ray redBottomRay = new Ray(playerEye.position, portalRed.Bottom - playerEye.position);
        Ray blueTopRay = new Ray(playerEye.position, portalBlue.Top - playerEye.position);
        Ray blueBottomRay = new Ray(playerEye.position, portalBlue.Bottom - playerEye.position);
        Debug.DrawRay(redTopRay.origin, redTopRay.direction * 100f, Color.red);
        Debug.DrawRay(redBottomRay.origin, redBottomRay. direction * 100f, Color.red);
        Debug.DrawRay(blueTopRay.origin, blueTopRay.direction * 100f, Color.blue);
        Debug.DrawRay(blueBottomRay.origin, blueBottomRay.direction * 100f, Color.blue);
        Debug.DrawLine(playerCamera.transform.position, portalRed.transform.position, Color.red);
        Debug.DrawLine(portalRed.portalCamera.transform.position, portalBlue.transform.position, Color.red);
        Debug.DrawLine(playerCamera.transform.position, portalBlue.transform.position, Color.blue);
        Debug.DrawLine(portalBlue.portalCamera.transform.position, portalRed.transform.position, Color.blue);
    }

    public void SetPortalRed(Vector3 position, Quaternion rotation)
    {
        portalRed.transform.SetPositionAndRotation(position, rotation);
    }
    public void SetPortalBlue(Vector3 position, Quaternion rotation)
    {
        portalBlue.transform.SetPositionAndRotation(position, rotation);
    }

    

    
    
}
