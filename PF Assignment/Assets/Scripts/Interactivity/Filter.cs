using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    public Collider filterCollider;
    public GameManager.Color color;

    void Awake()
    {
        filterCollider = GetComponent<Collider>();
        if ( ! filterCollider) Debug.LogError("Could not find Collider");

        GameManager.instance.filters.Add(this);
    }

    void OnDestroy()
    {
        GameManager.instance.filters.Remove(this);
    }
}
