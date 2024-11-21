using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] Transform _block;
    [SerializeField] MeshRenderer _Point1;
    [SerializeField] MeshRenderer _point2;
    
    [SerializeField] float _moveSpeed = 2.5f;
    [SerializeField] bool _canMove = false;
    [SerializeField] bool _showPoints = true;
    
    Vector3 _startPos;
    
    void Awake()
    {
        if (!_showPoints)
        {
            _Point1.enabled = false;
            _point2.enabled = false;
        }
        
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
        
        Vector3 pointPosDiff = _point2.transform.position - _Point1.transform.position;
        _block.transform.position = _startPos + ((pointPosDiff / 2) * pos);
    }
}