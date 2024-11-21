using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reset : MonoBehaviour
{
    public UnityEvent OnAwake;
    
    void Awake()
    {
        OnAwake?.Invoke();
    }
}
