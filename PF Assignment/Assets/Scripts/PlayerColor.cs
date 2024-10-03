using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    Renderer playerRenderer;

    [SerializeField] GameManager.Color startColor = 0;

    bool r = false;
    bool g = false;
    bool b = false;

    void Awake()
    {
        playerRenderer = GetComponent<Renderer>();
        if (!playerRenderer) Debug.LogError("Could not find a Renderer component");

        playerRenderer.material = gameManager.GetMaterial(startColor);
    }

    void Start()
    {
        gameManager.AdaptFilters(startColor);
    }

    void OnTriggerEnter(Collider other)
    {
        Painter p = other.GetComponent<Painter>();
        if (!p) return;
        
        AddColor(p.color);
    }

    void AddColor(GameManager.Color color)
    {
        switch (color)
        {
            case GameManager.Color.black:
                r = false;
                g = false;
                b = false;
                playerRenderer.material = gameManager.GetMaterial(GameManager.Color.black);
                gameManager.AdaptFilters(color);
                return;

            case GameManager.Color.red:    r = true; break;
            case GameManager.Color.green:  g = true; break;
            case GameManager.Color.blue:   b = true; break;

            default: return;
        }

        float colorValue = 0;

        if (r) colorValue += (int)GameManager.Color.red;
        if (g) colorValue += (int)GameManager.Color.green;
        if (b) colorValue += (int)GameManager.Color.blue;

        playerRenderer.material = gameManager.GetMaterial((GameManager.Color)colorValue);

        gameManager.AdaptFilters(color);
    }
}
