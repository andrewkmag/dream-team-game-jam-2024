using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    #region Fields

    private static SceneChanger _instance;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        ScenesScrObj.OnTransition += ChangeScene;
    }


    private void OnDisable()
    {
        ScenesScrObj.OnTransition += ChangeScene;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    #region Methods

    private static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion
}