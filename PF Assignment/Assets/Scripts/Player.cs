using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float velocity = 1;
    [SerializeField] bool moveLeft = false;
    [SerializeField] float jumpStrength = 1;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("Rigidbody not found.");
    }

    void Update()
    {
        InputMovement();
    }

    void FixedUpdate()
    {
        DefaultMovement();
    }

    void DefaultMovement()
    {
        Vector3 toMove = new(velocity, rb.velocity.y, rb.velocity.z);

        if (moveLeft) toMove.x = -toMove.x;

        rb.velocity = toMove;
        //Debug.Log(rb.velocity);

        if (Physics.Raycast(transform.position, new Vector3(rb.velocity.x, 0, 0), out RaycastHit hitInfo, 0.5f))
        {
            if (hitInfo.transform)
            {
                if (hitInfo.transform.CompareTag("Structure"))
                {
                    moveLeft = !moveLeft;
                    Jump();
                }
            }
        }
    }

    void InputMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.anyKey) {
            Debug.Log("jump");
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
    }
}
