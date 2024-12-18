using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))] //cannot do because the rb is on a child object
public class MovingBlock : MonoBehaviour
{
    [SerializeField] Transform _block;
    [SerializeField] MeshRenderer _waypoint1;
    [SerializeField] MeshRenderer _waypoint2;
    
    [SerializeField] float _moveSpeed = 2.5f;
    [SerializeField] bool _canMove = false;
    [SerializeField] bool _showWaypoints = true;
    
    Rigidbody rb;
    Vector3 _startPos;
    
    void Awake()
    {
        if (!_showWaypoints)
        {
            _waypoint1.enabled = false;
            _waypoint2.enabled = false;
        }
        
        if (!_block.TryGetComponent<Rigidbody>(out rb)) Debug.LogError("MovingBlock: could not find it's childs Rigidbody");
        
        _startPos = _block.transform.position;
    }

    void FixedUpdate()
    {
        if (!_canMove) return;
        
        Movement();
    }
    
    void Movement()
    {
        float pos = Mathf.Sin(Time.time * _moveSpeed);
        
        Vector3 pointPosDiff = _waypoint2.transform.position - _waypoint1.transform.position;
        //_block.transform.position = _startPos + ((pointPosDiff / 2) * pos);
        rb.MovePosition(_startPos + ((pointPosDiff / 2) * pos));
    }
}