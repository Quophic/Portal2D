using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLevelPortalCheckZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PortalController controller = GameObject.FindWithTag("LevelTransitionPortalController").GetComponent<PortalController>();
            controller.InactivatePortal();
            MainMenu mainMenu = GameObject.FindWithTag("UI").GetComponent<MainMenu>();
            mainMenu.CloseMainMenu();
        }

    }
}
