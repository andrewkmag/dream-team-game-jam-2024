using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenesScrObj", menuName = "ScriptableObjects/SceneTransition")]
public class ScenesScrObj : ScriptableObject
{
    #region Fields

    [SerializeField] private string sceneName;
    [SerializeField] private float transitionTime;

    #endregion
    
    #region Events

    public delegate void StartAction(string sceneName);
    public delegate void StartTransition(string sceneName, float transition);
    public static event StartAction OnChange;
    public static event StartTransition OnTransition;
    
    #endregion

    #region Methods
    
    public void ChangeScene()
    {
        OnChange?.Invoke(sceneName);
    }
    
    public void TransitionScene()
    {
        OnTransition?.Invoke(sceneName,transitionTime);
    }

    #endregion
}
