using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : Emitter
{
    [SerializeField] Bullet _bulletPrefab;

    void Awake()
    {
        if (!_bulletPrefab) Debug.LogError("_bulletPrefab: _bulletPrefab is not assigned in the inspector.", gameObject);
    }

    void Update()
    {
        RunTimer();

        if (_timer <= 0)
        {
            Instantiate(_bulletPrefab, transform.position, transform.rotation);
            Debug.Log("run");
        }
    }
}
