using UnityEngine;

[CreateAssetMenu(fileName = "ScenesScrObj", menuName = "ScriptableObjects/SceneTransition")]
public class ScenesScrObj : ScriptableObject
{
    #region Fields

    [SerializeField] private string sceneName;
    
    [SerializeField] private ScenesScrObj nextScene;
    #endregion

    #region Properties

    public ScenesScrObj NextScene
    {
        get => nextScene;
        set => nextScene = value;
    }

    #endregion
    #region Events

    public delegate void StartSceneChange(ScenesScrObj sceneSo);
    public delegate void StartTransition(string sceneName);
    public static event StartSceneChange OnSceneChange;
    public static event StartTransition OnTransition;
    
    #endregion
    
    #region Methods
    
    public void TransitionScene()
    {
        Debug.Log($"Transition to {sceneName}");
        if(sceneName==null) return;
        OnSceneChange?.Invoke(this);
        OnTransition?.Invoke(sceneName);
    }
    
    public void TransitionNextScene()
    {
        Debug.Log($"Transition to next {nextScene.sceneName}");
        if(nextScene==null) return;
        OnSceneChange?.Invoke(this);
        OnTransition?.Invoke(sceneName);
    }

    #endregion
}
