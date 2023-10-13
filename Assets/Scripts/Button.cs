using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Activator 
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Activate();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Stop();
    }
}
