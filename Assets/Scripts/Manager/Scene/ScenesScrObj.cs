using UnityEngine;

[CreateAssetMenu(fileName = "ScenesScrObj", menuName = "ScriptableObjects/SceneTransition")]
public class ScenesScrObj : ScriptableObject
{
    #region Fields

    [SerializeField] private string sceneName;
    [SerializeField] private float transitionTime;
    [SerializeField] private float gravity;
    [SerializeField] private int[] planets;
    #endregion

    #region Properties

    public float Gravity
    {
        get => gravity;
        set => gravity = value;
    }
    
    public int[] Planets
    {
        get => planets;
        set => planets = value;
    }

    #endregion
    
    #region Events

    public delegate void StartSceneChange(ScenesScrObj sceneSo);
    public delegate void StartAction(string sceneName);
    public delegate void StartTransition(string sceneName, float transition);
    public static event StartSceneChange OnSceneChange;
    public static event StartAction OnChange;
    public static event StartTransition OnTransition;
    
    #endregion

    #region Methods
    
    public void ChangeScene()
    {
        OnSceneChange?.Invoke(this);
        OnChange?.Invoke(sceneName);
    }
    
    public void TransitionScene()
    {
        OnSceneChange?.Invoke(this);
        OnTransition?.Invoke(sceneName,transitionTime);
    }

    #endregion
}
