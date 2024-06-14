using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenesScrObj", menuName = "ScriptableObjects/SceneTransition")]
public class ScenesScrObj : ScriptableObject
{
    #region Fields

    [SerializeField] private string sceneName;

    #endregion
    
    #region Events

    public delegate void StartAction(string sceneName);
    public static event StartAction OnChange;
    
    #endregion

    #region Methods
    
    public void ChangeScene()
    {
        OnChange?.Invoke(sceneName);
    }

    #endregion
}
