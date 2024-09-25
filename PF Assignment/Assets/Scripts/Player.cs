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

    void FixedUpdate()
    {
        Vector3 toMove = new(velocity, rb.velocity.y, rb.velocity.z);

        if (moveLeft) toMove = -toMove;

        rb.velocity = toMove;
        Debug.Log(rb.velocity);

        if (Physics.Raycast(transform.position, new Vector3(rb.velocity.x, 0, 0), out RaycastHit hitInfo, 0.5f))
        {
            if (hitInfo.transform)
            {
                if (hitInfo.transform.CompareTag("Structure"))
                {
                    moveLeft = !moveLeft;
                    rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                }
            }
        }
    }
}
