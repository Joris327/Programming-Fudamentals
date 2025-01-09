using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] float _velocity = 1;
    [SerializeField] UnityEvent _onWallHit;

    void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out _rb)) Debug.LogError("Bullet: Could not find Rigidbody component.", this);
    }

    void Update()
    {
        _rb.MovePosition(transform.position + (_velocity * Time.deltaTime * transform.forward));
    }

    void OnCollisionEnter(Collision collision)
    {
        _onWallHit.Invoke();
    }
}
