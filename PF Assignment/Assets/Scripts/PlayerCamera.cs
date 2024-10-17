using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Transform player;
    Vector3 lastPos;

    [SerializeField, Range(0,1)] float followFraction = 0.95f;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>().gameObject.transform;
        if (!player) Debug.Log("Camera cannot find the player.");

        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 playerPos = new(player.position.x, player.position.y, transform.position.z);

        transform.position = (playerPos * (1 - followFraction)) + (lastPos * followFraction);

        lastPos = transform.position;
    }
}
