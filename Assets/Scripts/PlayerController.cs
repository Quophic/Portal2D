using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Camera mainCam;
    public GameObject body;
    public Transform eye;
    public float power;
    public float jumpSpeed;
    public float horizontalMaxSpped;
    public float verticalMaxSpeed;
    public float returnSpeed;
    public PortalGun gun;
    void Update()
    {
        Stand();
        if (Input.GetMouseButtonDown(0))
        {
            gun.SetPortalRed();
        }
        if (Input.GetMouseButtonDown(1))
        {
            gun.SetPortalBlue();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (gun.Grabed)
            {
                gun.Put();
            }
            else
            {
                gun.Grab();
            }
        }
    }
    private void FixedUpdate()
    {
        Move();
        Jump();
        Face();

    }

    private void Stand()
    {
        Vector3 r = transform.rotation.eulerAngles;
        r.z = 0;
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(r), returnSpeed * Time.deltaTime);
        transform.RotateAround(eye.position, transform.forward, newRotation.eulerAngles.z - transform.rotation.eulerAngles.z);
    }

    private void Face()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mousePos - (Vector2)transform.position;
        bool faceRight = Vector2.Dot(toMouse, transform.right) >= 0;
        if (faceRight)
        {
            body.transform.localRotation = Quaternion.identity;
        }
        else
        {
            body.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        Vector2 force = mainCam.transform.right * h * power;
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
        if (Input.GetKeyDown(KeyCode.Space) && rb2d.velocity.y <= 0.00001f)
        {
            Vector2 velocity = Vector2.up  * jumpSpeed;
            velocity.x = rb2d.velocity.x;
            rb2d.velocity = velocity;
        }
    }

    
}
