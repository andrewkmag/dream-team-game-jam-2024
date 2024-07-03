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
    public static event StartSceneChange OnSceneChange;
    
    #endregion
    
    #region Methods
    
    public void LoadThisScene()
    {
        if(sceneName==null) return;
        OnSceneChange?.Invoke(this);
    }
    
    public void LoadNextScene()
    {
        if(nextScene==null) return;
        nextScene.LoadThisScene();
    }

    #endregion
}
