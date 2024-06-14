using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField] private ScenesScrObj actualScene;

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

    private void Start()
    {
        if (actualScene == null)
        {
            Debug.LogError($"Add a scriptable scene object to the Game Manager to Start");
        }
    }

    private void Reset()
    {
        if (actualScene == null)
        {
            Debug.LogWarning($"Add a scriptable scene object to the Game Manager to Start");
        }
    }
}
