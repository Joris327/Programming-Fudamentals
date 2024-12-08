using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    public GameManager.Color color;

    void Awake()
    {
        GameManager.painters.Add(this);
    }

    void OnDestroy()
    {
        GameManager.painters.Remove(this);
    }
}
