using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private ScenesScrObj actualScene;
    [SerializeField] private bool isPaused;
    [SerializeField] private bool isDead;
    [SerializeField] private Vector3 checkpointPosition;
    [SerializeField] private int requiredItemsRemaining;
    [SerializeField] private int maxItemsRequired;
    [SerializeField] private bool requisiteAchieved;
    [SerializeField] private ScenesScrObj nextScene;

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
    public int RequiredItemsRemaining
    {
        get => requiredItemsRemaining;
        set => requiredItemsRemaining = value;
    }
    public int MaxItemsRequired
    {
        get => maxItemsRequired;
        set => maxItemsRequired = value;
    }
    
    public bool RequisiteAchieved
    {
        get => requisiteAchieved;
        set => requisiteAchieved = value;
    }

    #endregion

    #region Constants

    const float PAUSED_TIME = 0;
    const float NO_REMAINING = 0;
    const float RESUMED_TIME = 1;
    private const int LEAVE_OPTION = 0;


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
    public static event Action OnSpawn;

    public static event Action OnPause;

    public static event Action OnResume;

    public static event Action OnUpdateJamCount;
    
    public static event Action OnUpdateSibling;

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
        HealthManager.OnDeath -= PlayerDeath;
        HealthManager.OnRespawn -= PlayerRespawn;
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

        if (nextScene == null)
        {
            Debug.LogWarning("GameManager needs an scene script");
        }
    }

    private void Start()
    {
        if (actualScene == null)
        {
            Debug.LogError($"Add a scriptable scene object to the Game Manager to Start");
        }
        IsPaused = false;
        PlayerSpawn();
    }

    #endregion

    #region Methods

    public void CollectedItem(int val)
    {
        requiredItemsRemaining = val;
        maxItemsRequired=(Mathf.Max(maxItemsRequired,val));
        OnUpdateJamCount?.Invoke();
        ReadyToLeave();
    }

    public void RequisiteAchived(bool val)
    {
        requisiteAchieved = val;
        OnUpdateSibling?.Invoke();
        ReadyToLeave();
    }

    public void ReadyToLeave()
    {
        if (!(requiredItemsRemaining <= NO_REMAINING) || !requisiteAchieved) return;
        var button = ContextualUIManager.Instace.ShowContextualOption(
            "You collected all the items and acomplished your mission",
            "Leave now",
            "Use the ship");
        button[LEAVE_OPTION].onClick.AddListener(TransitionToNextScene);
    }

    public void SpaceshipUse()
    {
        if (!(requiredItemsRemaining <= NO_REMAINING) || !requisiteAchieved)
        {
            var button = ContextualUIManager.Instace.ShowContextualButton(
                "You need to acomplish your missions first",
                "Understood");
        }
        else
        {
            var button = ContextualUIManager.Instace.ShowContextualButton(
                "You collected all the items and acomplished your mission",
                "Leave now");
            button.onClick.AddListener(TransitionToNextScene);
        }
    }

    private void TransitionToNextScene()
    {
        if(nextScene==null) return;
        nextScene.TransitionScene();
        OnUpdateJamCount?.Invoke();
        OnUpdateSibling?.Invoke();
        PlayerSpawn();
    }

    private void GetSceneValues(ScenesScrObj sceneSo)
    {
        actualScene = sceneSo;
        nextScene = sceneSo.NextScene; // We can add any number of variables and load them from the controllers
        IsDead = false;
    }

    private void PlayerDeath()
    {
        if (IsDead) return;
        IsDead = true;
        OnDeath?.Invoke();
        PauseGame();
        var button = ContextualUIManager.Instace.ShowContextualButton("You died", "Respawn");
        button.onClick.AddListener(() => HealthManager.Instance.Respawn());
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
    
    private void PlayerSpawn()
    {
        OnRespawn?.Invoke();
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