using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed;
    public float jumpSpeed;
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
        Vector2 velocity = Vector2.right * h * speed;
        velocity.y = rb2d.velocity.y;
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
