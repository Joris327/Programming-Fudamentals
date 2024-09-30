using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    Material playerMaterial;
    
    void Awake()
    {
        playerMaterial = Resources.Load<Material>("Materials/Player");
        playerMaterial.color = Color.black;
    }

    void OnTriggerEnter(Collider other)
    {
        Painter p = other.GetComponent<Painter>();
        if (!p) return;

        switch (p.color)
        {
            case Painter.Color.green:
                playerMaterial.color = Color.green;
                break;
        } 
    }
}
