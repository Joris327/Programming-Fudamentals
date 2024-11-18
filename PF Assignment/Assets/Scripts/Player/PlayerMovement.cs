using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] GameObject particlesMoveleft;
    [SerializeField] GameObject particlesMoveRight;

    [Header("Movement")]
    [SerializeField] float speed = 5;
    [SerializeField] bool moveLeft = false;
    bool hitWall = false;
    readonly Vector3 wallCollisionOffset = new(0, 0.4f, 0);

    [Header("Jumping")]
    bool jumpInput = false;
    [SerializeField, Tooltip("How high the player will jump when getting input from the keyboard")] float inputJumpStrength = 10.5f;
    [SerializeField, Tooltip("How high the player will jump when hitting a wall")] float wallJumpStrength = 7.7f;
    [SerializeField] float yVelocityLimit = 13;
    [SerializeField] int baseJumpHeap = 2;
    int jumpHeap;
    readonly Vector3 groundCollisionOffset = new(0.4f, 0, 0);

    [Header("Other")]
    [SerializeField] Vector3 displayVelocity;
    const float raycastDistance = 0.501f;
    Vector3 lastPos = new();
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("Rigidbody not found.");

        jumpHeap = baseJumpHeap;
    }

    void FixedUpdate()
    {
        VerticalMovement();
        HorizontalMovement();

        lastPos = transform.position;
        displayVelocity = rb.velocity;
    }

    void VerticalMovement()
    {
        if (hitWall)
        {
            Jump(false);
            hitWall = false;
        }

        if (IsCollidingWithWall() ||
            lastPos == transform.position) // <- if we haven't moved since last frame we are probably stuck in a corner and we should reverse course.
        {
            moveLeft = !moveLeft;
            hitWall = true;

            //particlesMoveleft.SetActive(moveLeft);
            //particlesMoveRight.SetActive(!moveLeft);
        }

        if (IsGrounded())
        {
            jumpHeap = baseJumpHeap;
        }

        if (jumpInput && jumpHeap > 0) Jump(true);
    }

    void HorizontalMovement()
    {
        Vector3 toMove = new(speed, rb.velocity.y, rb.velocity.z);
        if (moveLeft) toMove.x = -toMove.x;
        rb.velocity = toMove;

        if (rb.velocity.y > yVelocityLimit) //clamp positive Y velocity to prevent player from jumping way higher than desired.
        {
            Vector3 velocity = rb.velocity;
            velocity.y = yVelocityLimit;
            rb.velocity = velocity;
        }
    }

    bool IsCollidingWithWall()
    {
        Vector3 direction = new(rb.velocity.x, 0, 0);
        if (Raycast(transform.position + wallCollisionOffset, direction)) return true;
        if (Raycast(transform.position - wallCollisionOffset, direction)) return true;

        return false;
    }

    bool IsGrounded()
    {
        if (Raycast(transform.position + groundCollisionOffset, Vector3.down)) return true;
        if (Raycast(transform.position - groundCollisionOffset, Vector3.down)) return true;

        return false;
    }

    bool Raycast(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastDistance))
        {
            if (hitInfo.transform.CompareTag("Structure"))
            {
                return true;
            }
        }

        //Debug.DrawRay(origin, direction * raycastDistance, Color.red, 1);
        
        return false;
    }

    void Jump(bool fromPlayerInput)
    {
        if (!fromPlayerInput)
        {
            rb.AddForce(Vector3.up * wallJumpStrength, ForceMode.Impulse);
            return;
        }

        if (rb.velocity.y < 0)
        //if falling down: set Y to zero so that when we apply jumpforce we actually go up.
        {
            rb.velocity = new(
                rb.velocity.x,
                0,
                rb.velocity.z
            );
        }

        rb.AddForce(Vector3.up * inputJumpStrength, ForceMode.Impulse);

        jumpHeap--;
        jumpInput = false;
    }

    public void OnJump(InputValue v)
    {
        if (v.Get<float>() > 0 && jumpHeap > 0)
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

        if (other.CompareTag("Finish"))
        {
            GameManager.instance.SceneNavigation();
        }
    }

    public void OnPause(InputValue v)
    {
        GameManager.instance.SceneNavigation();
    }
}
