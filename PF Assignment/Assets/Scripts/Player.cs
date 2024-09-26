using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float speed = 5;
    [SerializeField] float jumpStrength = 5;
    [SerializeField] bool moveLeft = false;
    [SerializeField] int baseAmountOfJumps = 2;
    [SerializeField] int amountOfJumps;
    [SerializeField] Vector3 velocity;
    const float raycastDistance = 0.501f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("Rigidbody not found.");

        amountOfJumps = baseAmountOfJumps;
    }

    void Update()
    {
        if (IsGrounded()) amountOfJumps = baseAmountOfJumps;
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

        if (WallCollision(-0.5f) || WallCollision(0) || WallCollision(0.5f))
        {
            moveLeft = !moveLeft;
            Jump();
        }

        velocity = rb.velocity;
    }

    bool WallCollision(float yPosDelta)
    {
        Vector3 origin = transform.position;
        origin.y += yPosDelta;

        if (Physics.Raycast(origin, new Vector3(rb.velocity.x, 0, 0), out RaycastHit hitInfo, raycastDistance))
        {
            if (hitInfo.transform.CompareTag("Structure"))
                return true;
        }
        
        return false;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, raycastDistance))
            return true;

        return false;
    }

    public void OnJump(InputValue value)
    {
        if (value.Get<float>() != 0 && amountOfJumps > 0) 
        {
            Jump();
            amountOfJumps--;
        }
    }
}
