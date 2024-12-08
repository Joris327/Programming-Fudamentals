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
    [SerializeField] float _speed = 5;
    [SerializeField] bool _moveLeft = false;
    [SerializeField] bool _collisionDetectionThroughRaycast = false;
    [SerializeField] Vector3 _wallBoxcastHalfExtends = new(0.5f, 0.4f, 0.5f);
    [SerializeField] float _wallBoxcastDistance = 0.1f;
    bool _hitWall = false;

    [Header("Jumping")]
    bool _jumpInput = false;
    [SerializeField, Tooltip("How high the player will jump when getting input from the keyboard")] float _inputJumpStrength = 10.5f;
    [SerializeField, Tooltip("How high the player will jump when hitting a wall")] float _wallJumpStrength = 7.7f;
    [SerializeField] float _yVelocityLimit = 12;
    [SerializeField] int _baseJumpHeap = 2;
    int _jumpHeap;
    [SerializeField] Vector3 _groundBoxcastHalfExtends = new(0.4f, 0.5f, 0.5f);

    [Header("Other")]
    int _pickupsCount = 0;
    [SerializeField] Vector3 _displayVelocity;
    Vector3 _lastPos = new();
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("Rigidbody not found.");

        _jumpHeap = _baseJumpHeap;
    }

    void FixedUpdate()
    {
        if (_lastPos == rb.position)
        {
            _moveLeft = !_moveLeft;
            _hitWall = true;
        }
        
        VerticalMovement();
        HorizontalMovement();

        _lastPos = transform.position;
        _displayVelocity = rb.velocity;
    }
    Vector3 beforevel;
    void HorizontalMovement()
    {
        //Vector3 toMove = new(_speed, rb.velocity.y, rb.velocity.z);
        Vector3 toMove = new(_speed  * Time.fixedDeltaTime, 0, 0);
        if (_moveLeft) toMove.x = -toMove.x;
        toMove.x -= rb.velocity.x;
        
        rb.AddForce(toMove, ForceMode.VelocityChange);
    }

    void VerticalMovement()
    {
        if (_hitWall)
        {
            Jump(false);
            _hitWall = false;
        }

        if (_collisionDetectionThroughRaycast)
        {
            Vector3 castDirection = new(rb.velocity.x, 0, 0);
            if (Raycast(_wallBoxcastHalfExtends, castDirection) || //checks if we collided with a wall
                _lastPos == transform.position) // <- if we haven't moved since last frame we are probably stuck in a corner and we should reverse course.
            {
                _moveLeft = !_moveLeft;
                _hitWall = true;

                //particlesMoveleft.SetActive(moveLeft);
                //particlesMoveRight.SetActive(!moveLeft);
            }
            
            if (Raycast(_groundBoxcastHalfExtends, Vector3.down)) //if: grounded
            {
                _jumpHeap = _baseJumpHeap;
            }
        }

        if (_jumpInput)
        {
            Jump(true);
        }
        
        if (rb.velocity.y > _yVelocityLimit) //clamp positive Y velocity to prevent player from jumping way higher than desired.
        {
            rb.AddForce(0, _yVelocityLimit - rb.velocity.y, 0, ForceMode.VelocityChange);
            //Vector3 velocity = rb.velocity;
            //velocity.y = _yVelocityLimit;
            //rb.velocity = velocity;
        }
    }

    bool Raycast(Vector3 boxcastHalfExtends, Vector3 castDirection)
    {
        if (Physics.BoxCast(transform.position, boxcastHalfExtends, castDirection, out RaycastHit hitInfo, Quaternion.identity, _wallBoxcastDistance))
        {
            if (!hitInfo.transform.gameObject.CompareTag("Structure")) return false;
                
            Vector3 collisionDirection = hitInfo.point - transform.position;
            float angle = Vector3.Angle(transform.position, collisionDirection);
            if (angle != 180) return true;
        }
        
        return false;
    }

    void Jump(bool fromPlayerInput)
    {
        if (!fromPlayerInput)
        {
            rb.AddForce(Vector3.up * _wallJumpStrength, ForceMode.Impulse);
            return;
        }

        if (rb.velocity.y < 0) //if falling down: set Y to zero so that when we apply jumpforce we actually go up.
        {
            //rb.velocity = new(
            //    rb.velocity.x,
            //    0,
            //    rb.velocity.z
            //);
            rb.AddForce(Vector3.up * -rb.velocity.y, ForceMode.Impulse);
        }

        rb.AddForce(Vector3.up * _inputJumpStrength, ForceMode.Impulse);
        _jumpHeap--;
        _jumpInput = false;
    }

    public void OnJump(InputValue v)
    {
        if (v.Get<float>() > 0 && _jumpHeap > 0)
        {
            _jumpInput = true;
        }
    }

    public void OnPause(InputValue v) //Does it makes sense that this is here? no.  Is it the only thing that workes for some reason? yes.
    {
        UIManager.Instance.SwitchPaused();
    }
    
    void OnCollisionEnter(Collision other)
    {
        HandleCollisions(other);
    }

    void HandleCollisions(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        
        if (!contact.otherCollider.CompareTag("Structure")) return;
        
        if (contact.normal.y == 1)
        {
            _jumpHeap = _baseJumpHeap;
        }
        else if (contact.normal.x == -1)
        {
            _moveLeft = true;
            _hitWall = true;
        }
        else if (contact.normal.x == 1)
        {
            _moveLeft = false;
            _hitWall = true;
        }
        
        Debug.DrawRay(contact.point, contact.normal, Color.white);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            _pickupsCount++;
            UIManager.Instance.UpdatePickupText(_pickupsCount);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Finish"))
        {
            GameManager.LoadNextScene();
        }
    }
}
