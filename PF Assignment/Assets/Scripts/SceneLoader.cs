using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoader.asset", menuName = "ScriptableObjects/Singletons/SceneLoader")]
public class SceneLoader : ScriptableObject
{
    public static void Quit()
    {
        Application.Quit();
    }

    public static void LoadScene(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public static void LoadNextScene()
    {
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        if (sceneToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            LoadScene(0);
            return;
        }

        LoadScene(sceneToLoad);
    }
}
