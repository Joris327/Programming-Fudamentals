using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] float speed = 5;
    [SerializeField] bool moveLeft = false;
    const float wallCollisionOffset = 0.4f;

    [Header("Jumping")]
    [SerializeField] bool jumpInput = false;
    [SerializeField] float jumpStrength = 8;
    [SerializeField] float yVelocityLimit = 10;
    [SerializeField] int baseJumpHeap = 2;
    [SerializeField] int jumpHeap;
    const float groundCollisionOffset = 0.4f;

    [Header("Other")]
    [SerializeField] Vector3 displayVelocity;
    const float raycastDistance = 0.501f;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("Rigidbody not found.");

        jumpHeap = baseJumpHeap;
    }

    void FixedUpdate()
    {
        DefaultMovement();
    }

    void DefaultMovement()
    {
        Vector3 toMove = new(speed, rb.velocity.y, rb.velocity.z);

        if (moveLeft) toMove.x = -toMove.x;

        rb.velocity = toMove;

        //if (colliding with wall)
        if (Raycast(new Vector3(0,  wallCollisionOffset, 0), new Vector3(rb.velocity.x, 0, 0)) ||
            Raycast(new Vector3(0, -wallCollisionOffset, 0), new Vector3(rb.velocity.x, 0, 0)))
        {
            moveLeft = !moveLeft;
            Jump();
        }

        //if (Grounded)
        if (Raycast(new Vector3( groundCollisionOffset,  0, 0), Vector3.down) ||
            Raycast(new Vector3(-groundCollisionOffset,  0, 0), Vector3.down))
        {
            jumpHeap = baseJumpHeap;
        }

        if (jumpInput && jumpHeap > 0)
        {
            jumpHeap--;

            Jump();
        }

        jumpInput = false;

        if (rb.velocity.y > yVelocityLimit)
        {
            Vector3 velocity = rb.velocity;
            velocity.y = yVelocityLimit;
            rb.velocity = velocity;
        }

        displayVelocity = rb.velocity;
    }

    bool Raycast(Vector3 originOffset, Vector3 direction)
    {
        Vector3 origin = transform.position;
        origin += originOffset;
        
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastDistance))
        {
            if (hitInfo.transform.CompareTag("Structure"))
            {
                return true;
            }
        }
        
        return false;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }

    public void OnJump(InputValue v)
    {
        if (v.Get<float>() > 0)
        {
            jumpInput = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            GameManager.instance.IncrementPickupCount();
            Destroy(other.gameObject);
        }
    }
}
