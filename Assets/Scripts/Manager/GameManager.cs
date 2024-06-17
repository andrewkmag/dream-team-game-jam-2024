using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    private static GameManager _instance;
    [SerializeField] private ScenesScrObj actualScene;
    [SerializeField] private float gravity;
    [SerializeField] private bool isdead;

    #endregion

    #region Events

    public delegate void DieAction();

    public static event DieAction OnDeath;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        ScenesScrObj.OnSceneChange += GetSceneValues;
        HealthManager.OnDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        ScenesScrObj.OnSceneChange -= GetSceneValues;
        HealthManager.OnDeath += PlayerDeath;
    }

    private void Reset()
    {
        if (actualScene == null)
        {
            Debug.LogWarning($"Add a scriptable scene object to the Game Manager to Start");
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            GetSceneValues(actualScene);
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

    #endregion

    #region Methods

    private void GetSceneValues(ScenesScrObj sceneSo)
    {
        actualScene = sceneSo;
        gravity = sceneSo.Gravity; // We can add any number of variables and load them from the controllers
        isdead = false;
    }

    private void PlayerDeath()
    {
        isdead = true;
        OnDeath?.Invoke();
    }

    #endregion
}