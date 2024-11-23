using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstant : MonoBehaviour
{
    [SerializeField] Vector3 _direction = new( 1, 1, 1);
    [SerializeField] float _speed = 5f;

    void FixedUpdate()
    {
        transform.Rotate(_direction, _speed * Time.fixedDeltaTime);
    }
}
