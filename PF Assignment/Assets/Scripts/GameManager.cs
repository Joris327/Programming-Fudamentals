using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Color {
        black = 0,
        red = 2,
        green = 3,
        blue = 4,
        orange = 5,
        purple = 6,
        yellow = 7,
        white = 9,
    }

    public List<Filter> filters = new();

    public Dictionary<Color, Material> coloredMaterials = new();

    void Awake()
    {
        coloredMaterials.Add(Color.black,  Resources.Load<Material>("Pixel Colors/Black"));
        coloredMaterials.Add(Color.red,    Resources.Load<Material>("Pixel Colors/Red"));
        coloredMaterials.Add(Color.green,  Resources.Load<Material>("Pixel Colors/Green"));
        coloredMaterials.Add(Color.blue,   Resources.Load<Material>("Pixel Colors/Blue"));
        coloredMaterials.Add(Color.orange, Resources.Load<Material>("Pixel Colors/Orange"));
        coloredMaterials.Add(Color.purple, Resources.Load<Material>("Pixel Colors/Purple"));
        coloredMaterials.Add(Color.yellow, Resources.Load<Material>("Pixel Colors/Yellow"));
        coloredMaterials.Add(Color.white,  Resources.Load<Material>("Pixel Colors/White"));
    }

    public Material GetMaterial(Color color)
    {
        return coloredMaterials[color];
    }

    public void AdaptFilters(Color color)
    {
        foreach (Filter f in filters)
        {
            if (f.color == color) f.filterCollider.enabled = false;
            else f.filterCollider.enabled = true;
        }
    }
}
