using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ExitLevelPortalController : Effector
{
    private PortalController controller;

    private void Awake()
    {
        onActivated += OpenExitPortal;
        onStopped += CloseExitPortal;
    }

    private void OpenExitPortal()
    {
        if(controller == null)
        {
            controller = GameObject.FindWithTag("LevelTransitionPortalController").GetComponent<PortalController>();
            LevelManager.Instance.LoadNextLevel();
            StartCoroutine(OpenPortal());
        }
        else
        {
            controller.ActivatePortal();
        }
    }
    IEnumerator OpenPortal()
    {
        yield return new WaitForSeconds(0.01f);
        LevelManager.Instance.OpenTransitionPortal();
    }
    private void CloseExitPortal()
    {
        if (controller)
        {
            controller.InactivatePortal();
        }
    }
}
