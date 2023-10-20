using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Activator
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
