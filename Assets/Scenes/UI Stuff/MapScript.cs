using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScript : MonoBehaviour
{
    public void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Credits")
        {
            StartCoroutine(LoadMainMenu());
        }
    }

    
    public void loadingNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(23);
        SceneManager.LoadScene("MainMenu");
    }
}
