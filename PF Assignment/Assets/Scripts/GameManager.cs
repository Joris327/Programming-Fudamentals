using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "GameManager.asset", menuName = "ScriptableObjects/Singletons/GameManager")]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector] public static List<Filter> filters = new();

    public Dictionary<Color, Material> coloredMaterials = new();

    public enum Color { 
        black = 0,
        red = 2,
        green = 3,
        blue = 4,
        yellow = 5,
        magenta = 6,
        cyan = 7,
        white = 9,
    }

    public void Awake()
    {
        Setup();
    }

    void Setup()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more then one GameManager in the scene, the duplicate will destroy itself");
            Destroy(this);
        }
        else Instance = this;
        
        coloredMaterials.Add(Color.black,  Resources.Load<Material>("Player Materials/Black"));
        coloredMaterials.Add(Color.red,    Resources.Load<Material>("Player Materials/Red"));
        coloredMaterials.Add(Color.green,  Resources.Load<Material>("Player Materials/Green"));
        coloredMaterials.Add(Color.blue,   Resources.Load<Material>("Player Materials/Blue"));
        coloredMaterials.Add(Color.yellow, Resources.Load<Material>("Player Materials/Yellow"));
        coloredMaterials.Add(Color.magenta, Resources.Load<Material>("Player Materials/Magenta"));
        coloredMaterials.Add(Color.cyan, Resources.Load<Material>("Player Materials/Cyan"));
        coloredMaterials.Add(Color.white,  Resources.Load<Material>("Player Materials/White"));

        Debug.Log("Gamemanager Enabled at: " + Time.time);
    }

    public Material GetMaterial(Color color)
    {
        Debug.Log(color);
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
}
