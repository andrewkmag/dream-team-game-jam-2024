using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScript : MonoBehaviour
{
    public void Update()
    {
        StartCoroutine(LoadMainMenu());
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSecondsRealtime(24);
        SceneManager.LoadScene("MainMenu");
    }
}
