using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    GameManager gameManager;
    Renderer playerRenderer;

    [SerializeField] GameManager.Color startColor = 0;

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
        gameManager = GameManager.instance;
        
        playerRenderer.material = gameManager.GetMaterial(startColor);
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
                SetColor(GameManager.Color.black);
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

        SetColor((GameManager.Color)colorValue);
    }

    void SetColor(GameManager.Color color)
    {
        playerRenderer.material = gameManager.GetMaterial(color);

        gameManager.AdaptFilters(color);
    }
}
