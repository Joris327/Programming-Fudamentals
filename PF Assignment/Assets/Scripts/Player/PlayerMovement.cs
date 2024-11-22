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
        VerticalMovement();
        HorizontalMovement();

        _lastPos = transform.position;
        _displayVelocity = rb.velocity;
    }
    
    void HorizontalMovement()
    {
        Vector3 toMove = new(_speed, rb.velocity.y, rb.velocity.z);
        if (_moveLeft) toMove.x = -toMove.x;
        rb.velocity = toMove;

        if (rb.velocity.y > _yVelocityLimit) //clamp positive Y velocity to prevent player from jumping way higher than desired.
        {
            Vector3 velocity = rb.velocity;
            velocity.y = _yVelocityLimit;
            rb.velocity = velocity;
        }
    }

    void VerticalMovement()
    {
        if (_hitWall)
        {
            Jump(false);
            _hitWall = false;
        }
        
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

        if (_jumpInput && _jumpHeap > 0)
        {
            Jump(true);
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
            rb.velocity = new(
                rb.velocity.x,
                0,
                rb.velocity.z
            );
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
            SceneLoader.LoadNextScene();
        }
    }

    public void OnPause(InputValue v)
    {
        SceneLoader.LoadScene(0);
    }
}
