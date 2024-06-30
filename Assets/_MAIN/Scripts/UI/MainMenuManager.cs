using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace eXplorerJam.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        // Method to load a scene by name
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        // Method to load the next scene in the build order
        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Check if the next scene index is within the build settings range
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("Next scene index is out of range. Make sure you have added scenes to the Build Settings.");
            }
        }

        // Method to exit the game
        public void ExitGame()
        {
            // This will only work in a built application
            Application.Quit();

            // If you are testing in the editor, this will stop the play mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}