using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float power;
    public float jumpSpeed;
    public float horizontalMaxSpped;
    public float verticalMaxSpeed;
    public PortalGun gun;
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            gun.SetPortalRed();
        }
        if (Input.GetMouseButtonDown(1))
        {
            gun.SetPortalBlue();
        }
    }
    private void FixedUpdate()
    {
        Move();
        Jump();
        Face();
    }

    private void Face()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool aimRight = mousePos.x - transform.position.x > 0;
        if (aimRight)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        Vector2 force = Vector2.right * h * power;
        rb2d.AddForce(force);
        Vector2 velocity = rb2d.velocity;
        if(Mathf.Abs(velocity.x) > horizontalMaxSpped)
        {
            velocity.x = Mathf.Sign(velocity.x) * horizontalMaxSpped;
        }
        if(Mathf.Abs(velocity.y) > verticalMaxSpeed)
        {
            velocity.y = Mathf.Sign(velocity.y) * verticalMaxSpeed;
        }
        rb2d.velocity = velocity;
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rb2d.velocity.y == 0)
        {
            Vector2 velocity = Vector2.up  * jumpSpeed;
            velocity.x = rb2d.velocity.x;
            rb2d.velocity = velocity;
        }
    }

    
}
