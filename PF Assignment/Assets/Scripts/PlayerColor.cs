using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    Renderer playerRenderer;

    [SerializeField] Pixel.Color startColor = 0;

    bool r = false;
    bool g = false;
    bool b = false;

    void Awake()
    {
        playerRenderer = GetComponent<Renderer>();
        if (!playerRenderer) Debug.LogError("Could not find a Renderer component");
    }

    void Start()
    {
        playerRenderer.material = Pixel.GetMaterial(startColor);
    }

    void OnTriggerEnter(Collider other)
    {
        Painter p = other.GetComponent<Painter>();
        if (!p) return;
        
        AddColor(p.color);
    }

    void AddColor(Pixel.Color color)
    {
        switch (color)
        {
            case Pixel.Color.black:
                r = false;
                g = false;
                b = false;
                playerRenderer.material = Pixel.GetMaterial(Pixel.Color.black);
                return;

            case Pixel.Color.red:    r = true; break;
            case Pixel.Color.green:  g = true; break;
            case Pixel.Color.blue:   b = true; break;

            default: return;
        }

        float colorValue = 0;

        if (r) colorValue += (int)Pixel.Color.red;
        if (g) colorValue += (int)Pixel.Color.green;
        if (b) colorValue += (int)Pixel.Color.blue;

        playerRenderer.material = Pixel.GetMaterial((Pixel.Color)colorValue);
    }
}
