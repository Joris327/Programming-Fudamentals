using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameManager.asset", menuName = "ScriptableObjects/Singletons/GameManager")]
public class GameManager : ScriptableObject
{
    public static GameManager Instance { get; private set; }

    //these are static because some objects might try to add themselves before the instace variable has been set
    [HideInInspector] public static List<Filter> filters = new();
    [HideInInspector] public static List<Painter> painters = new();

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

    void Awake()
    {
        Setup();
        Debug.Log("GameManager enabled through Awake.");
    }
    
    void OnEnable()
    {
        Setup();
        Debug.Log("GameManager enabled through OnEnable.");
    }

    void Setup()
    {
        if (Instance != null && Instance == this) return;
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more then one GameManager in the scene");
            //Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            Debug.Log("GameManager Instance set");
        }
        
        coloredMaterials.Add(Color.black,  Resources.Load<Material>("Player Materials/Black"));
        coloredMaterials.Add(Color.red,    Resources.Load<Material>("Player Materials/Red"));
        coloredMaterials.Add(Color.green,  Resources.Load<Material>("Player Materials/Green"));
        coloredMaterials.Add(Color.blue,   Resources.Load<Material>("Player Materials/Blue"));
        coloredMaterials.Add(Color.yellow, Resources.Load<Material>("Player Materials/Yellow"));
        coloredMaterials.Add(Color.magenta,Resources.Load<Material>("Player Materials/Magenta"));
        coloredMaterials.Add(Color.cyan,   Resources.Load<Material>("Player Materials/Cyan"));
        coloredMaterials.Add(Color.white,  Resources.Load<Material>("Player Materials/White"));

        //Debug.Log("Gamemanager Enabled at: " + Time.time);
    }

    public Material GetMaterial(Color color)
    {
        if (coloredMaterials.Count == 0) Setup();
        return coloredMaterials[color];
    }

    public void AdaptFilters(Color color)
    {
        if (filters == null || filters.Count == 0) return;

        foreach (Filter f in filters)
        {
            if (f.Color == color) f.AllowPassage(true);
            else f.AllowPassage(false);
        }
    }

    public void AdaptPainters(Color color)
    {
        if (painters == null || painters.Count == 0) return;

        foreach(Painter f in painters)
        {
            Light light = f.GetComponent<Light>();
            if (!light) continue;

            if (color == Color.black)
            {
                light.enabled = true;
            }
            else if (color == f.color)
            {
                light.enabled = false;
            }
        }
    }

    public static void LoadNextScene()
    {
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        if (sceneToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(0);
            return;
        }

        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
