using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    #region Properties

    public static SceneChanger Instance { get; private set; }

    #endregion
    
    #region Unity Methods

    private void OnEnable()
    {
        ScenesScrObj.OnSceneChange += LoadScene;
    }


    private void OnDisable()
    {
        ScenesScrObj.OnSceneChange += LoadScene;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Methods

    private static void LoadScene(ScenesScrObj scenesScrObj)
    {
        SceneManager.LoadScene(scenesScrObj.name);
    }
    
    public void LoadGameManagerActualScene()
    {
        GameManager.Instance.ActualScene.LoadThisScene();
    }
    
    public void LoadGameManagerNextScene()
    {
        GameManager.Instance.ActualScene.LoadNextScene();
    }

    #endregion
}