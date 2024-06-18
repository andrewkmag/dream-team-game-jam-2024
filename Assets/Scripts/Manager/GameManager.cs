using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private ScenesScrObj actualScene;
    [SerializeField] private float gravity;
    [SerializeField] private bool isPaused;
    [SerializeField] private bool isDead;
    [SerializeField] private Vector3 checkpointPosition;

    #endregion

    #region Properties

    public static GameManager Instance { get; private set; }

    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }

    public Vector3 CheckpointPosition
    {
        get => checkpointPosition;
        set => checkpointPosition = value;
    }

    #endregion

    #region Constants

    const float PAUSED_TIME = 0;
    const float RESUMED_TIME = 1;


#if UNITY_EDITOR
    private const float SPHERE_RAD = 0.5f;
    private const int HANDLE_CONTROL = 0;

    private readonly Color _handleNodeColor = new Color(0, 255, 0);
#endif

    #endregion

    #region Events

    public delegate void Action();

    public static event Action OnDeath;
    public static event Action OnRespawn;

    public static event Action OnPause;

    public static event Action OnResume;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        ScenesScrObj.OnSceneChange += GetSceneValues;
        HealthManager.OnDeath += PlayerDeath;
        HealthManager.OnRespawn += PlayerRespawn;
    }

    private void OnDisable()
    {
        ScenesScrObj.OnSceneChange -= GetSceneValues;
        HealthManager.OnDeath += PlayerDeath;
        HealthManager.OnRespawn += PlayerRespawn;
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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            GetSceneValues(actualScene);
        }
        else
        {
            Destroy(this);
        }
        
        checkpointPosition=Vector3.zero;
    }

    private void Start()
    {
        if (actualScene == null)
        {
            Debug.LogError($"Add a scriptable scene object to the Game Manager to Start");
        }

        if (checkpointPosition == Vector3.zero)
        {
            checkpointPosition = transform.position;
        }
        IsPaused = false;
    }

    #endregion

    #region Methods

    private void GetSceneValues(ScenesScrObj sceneSo)
    {
        actualScene = sceneSo;
        gravity = sceneSo.Gravity; // We can add any number of variables and load them from the controllers
        IsDead = false;
    }

    private void PlayerDeath()
    {
        if (IsDead) return;
        IsDead = true;
        OnDeath?.Invoke();
        PauseGame();
        var button = ContextualUIManager.Instace.ShowContextualButton("You died", "Respawn");
        button.onClick.AddListener(()=>HealthManager.Instance.Respawn());
    }

    private void PauseGame()
    {
        IsPaused = !IsPaused;
        Time.timeScale = !IsPaused ? RESUMED_TIME : PAUSED_TIME;
        if (isPaused && !isDead)
        {
            OnPause?.Invoke();
        }
        else
        {
            OnResume?.Invoke();
        }
    }

    private void PlayerRespawn()
    {
        if (!IsDead) return;
        IsDead = false;
        OnRespawn?.Invoke();
        PauseGame();
    }

    #endregion

    #region Debug

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (checkpointPosition == null) return;
        Handles.color = _handleNodeColor;
        
        Handles.SphereHandleCap(HANDLE_CONTROL, checkpointPosition, Quaternion.identity,
            SPHERE_RAD,
            EventType.Repaint);
    }
#endif

    #endregion
}