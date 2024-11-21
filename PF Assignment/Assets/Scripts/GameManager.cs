using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "GameManager.asset", menuName = "ScriptableObjects/Singletons/GameManager")]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    int _pickupsCount = 0;
    public int PickupsCount { get{ return _pickupsCount; } }

    [HideInInspector] public static List<Filter> filters = new();

    public Dictionary<Color, Material> coloredMaterials = new();

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

    public void Awake()
    {
        Setup();
    }

    void Setup()
    {
        if (!Instance) Instance = this;
        
        coloredMaterials.Add(Color.black,  Resources.Load<Material>("Player Materials/Black"));
        coloredMaterials.Add(Color.red,    Resources.Load<Material>("Player Materials/Red"));
        coloredMaterials.Add(Color.green,  Resources.Load<Material>("Player Materials/Green"));
        coloredMaterials.Add(Color.blue,   Resources.Load<Material>("Player Materials/Blue"));
        coloredMaterials.Add(Color.orange, Resources.Load<Material>("Player Materials/Orange"));
        coloredMaterials.Add(Color.purple, Resources.Load<Material>("Player Materials/Purple"));
        coloredMaterials.Add(Color.yellow, Resources.Load<Material>("Player Materials/Yellow"));
        coloredMaterials.Add(Color.white,  Resources.Load<Material>("Player Materials/White"));

        Debug.Log("Gamemanager Enabled at: " + Time.time);
    }

    public Material GetMaterial(Color color)
    {
        if (coloredMaterials.Count == 0) Setup();
        return coloredMaterials[color];
    }

    public void AdaptFilters(Color color)
    {
        if (filters.Count == 0) Debug.LogWarning("GameManager: is not aware of any filters existing");
        foreach (Filter f in filters)
        {
            if (f.Color == color) f.AllowPassage(true);
            else f.AllowPassage(false);
        }
    }

    public void IncrementPickupCount()
    {
        _pickupsCount++;
    }
}
