using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pixel", menuName = "ScriptableObject/Pixel")]
public class Pixel : ScriptableObject
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

    static Dictionary<Color, Material> coloredMaterials = new(); //do NOT make readonly as VS Code recommends! It then won't add the materials to the dictionary properly

    void OnEnable()
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

    public static Material GetMaterial(Color color)
    {
        return coloredMaterials[color];
    }
}
