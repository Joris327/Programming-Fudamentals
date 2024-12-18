using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] RotateConstant pickupPrefab;
    
    void Awake()
    {
        Instantiate(pickupPrefab, transform.position, quaternion.identity);
        Destroy(gameObject);
    }
}
