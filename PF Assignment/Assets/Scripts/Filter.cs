using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    GameManager gameManager;

    public Collider filterCollider;
    public GameManager.Color color;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if ( ! gameManager) Debug.LogError("Could not find GameManager");

        filterCollider = GetComponent<Collider>();
        if ( ! filterCollider) Debug.LogError("Could not find Collider");
    }

    void Start()
    {
        gameManager.filters.Add(this);
    }

    void OnDestroy()
    {
        gameManager.filters.Remove(this);
    }
}
