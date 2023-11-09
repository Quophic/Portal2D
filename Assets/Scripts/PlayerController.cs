using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public GameObject body;
    public Transform eye;
    public float power;
    public float jumpSpeed;
    public float horizontalMaxSpped;
    public float verticalMaxSpeed;
    public float returnSpeed;
    public Transform gunSocket;
    public PortalGun gun;
    public SubPortalTraveller gunTraveller;
    public BoxCollider2D groundChecker;
    public PortalTraveller traveller;

    private void Awake()
    {
        traveller.OnTeleported += SetPlayerCamera;
    }

    void SetPlayerCamera(Matrix4x4 teleportMatrix)
    {
        //Vector3 rotation = teleportMatrix.rotation.eulerAngles;
        //rotation.x = 0;
        //rotation.z = 0;
        //eye.localRotation = Quaternion.Euler(rotation) * eye.localRotation;
    }

    void Update()
    {
        Aim();
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
        Jump();
    }
    private void FixedUpdate()
    {
        Move();
        
        Face();

    }

    private void Stand()
    {
        Vector3 r = transform.rotation.eulerAngles;
        r.z = 0;
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(r), returnSpeed * Time.deltaTime);
        transform.rotation = newRotation;
    }

    private void Face()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (traveller.teleported)
        {
            mousePos = traveller.closestPortal.TeleportMatrix.MultiplyPoint(mousePos);
        }
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
        Vector2 force = Camera.main.transform.right * h * power;
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
        if (Input.GetKeyDown(KeyCode.Space) && CheckOnGround())
        {
            Vector2 velocity = Vector2.up  * jumpSpeed;
            velocity.x = rb2d.velocity.x;
            rb2d.velocity = velocity;
        }
    }
    private void Aim()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (traveller.teleported)
        {
            mousePos = traveller.closestPortal.TeleportMatrix.MultiplyPoint(mousePos);
        }
        Vector3 aimDir = (Vector3)mousePos - gunSocket.position;
        gunSocket.right = aimDir;
        gunTraveller.CheckAndTeleport();
    }
    
    private bool CheckOnGround()
    {
        Transform t = groundChecker.transform;
        bool isClockWise = Vector3.Dot(transform.forward, Vector3.forward) >= 0;
        float zRotation = t.rotation.eulerAngles.z;
        zRotation = isClockWise ? zRotation : -zRotation;
        return Physics2D.BoxCast(t.position, groundChecker.size, zRotation, -t.up, 0, LayerMask.GetMask("Ground", "PortalRed", "NearProtalRed", "PortalBlue", "NearPortalBlue", "Dynamic"));
    }
}
