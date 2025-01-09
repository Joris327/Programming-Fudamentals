using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField] protected float _firerate = 0.5f;
    protected float _timer = 0; 

    protected void RunTimer()
    {
        if (_firerate == 0) return;

        if (_timer < 0) _timer = 1 /_firerate;

        _timer -= Time.deltaTime;
    }
}
