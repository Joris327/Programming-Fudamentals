using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoader.asset", menuName = "ScriptableObjects/Singletons/SceneLoader")]
public class SceneLoader : ScriptableObject
{
    public static SceneLoader Instance;

    public void OnEnable()
    {
        if (!Instance) Instance = this;
    }

    public void Awake()
    {
        if (!Instance) Instance = this;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void LoadNextScene()
    {
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        if (sceneToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            LoadScene(0);
            return;
        }

        LoadScene(sceneToLoad);
    }

    public void SceneNavigation() //very temporary code for navigating scenes
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 0)
        {
            Quit();
        }
        else if (currentScene == 1)
        {
            SceneManager.LoadSceneAsync(2);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
