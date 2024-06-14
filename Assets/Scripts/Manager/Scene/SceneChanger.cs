using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    #region UnityMethods

    private void OnEnable()
    {
        ScenesScrObj.OnChange += ChangeScene;
    }


    private void OnDisable()
    {
        ScenesScrObj.OnChange -= ChangeScene;
    }

    #endregion

    #region Methods

    private static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion
}