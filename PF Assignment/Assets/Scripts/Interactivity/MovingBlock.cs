using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] Transform _block;
    [SerializeField] MeshRenderer _Point1;
    [SerializeField] MeshRenderer _point2;
    
    [SerializeField] float _moveSpeed = 2.5f;
    bool _isTargetingPoint1 = true;
    [SerializeField] bool _move = false;
    
    void Awake()
    {
        //_Point1.enabled = false;
        //_point2.enabled = false;
    }

    void Update()
    {
        if (!_move) return;
        
        Vector3 targetPos;
        
        if (_isTargetingPoint1) targetPos = _Point1.transform.position;
        else targetPos = _point2.transform.position;
        
        float distanceToMove = _moveSpeed * Time.deltaTime;
        Vector3 delta = targetPos - _block.transform.position;
        
        if (delta.magnitude < distanceToMove)
        {
            _block.transform.position = targetPos;
            _isTargetingPoint1 = !_isTargetingPoint1;
        }
        else
        {
            _block.transform.Translate(delta.normalized * distanceToMove, Space.World);
        }
    }
}