using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent;
    public UnityEvent onTriggerStayEvent;
    public UnityEvent onTriggerExitEvent;
    
    void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        onTriggerEnterEvent?.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if (!enabled) return;
        onTriggerStayEvent?.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (!enabled) return;
        onTriggerExitEvent?.Invoke();
    }
}
