using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    private static GameManager _instance;
    [SerializeField] private ScenesScrObj actualScene;
    [SerializeField] private float gravity;
    [SerializeField] private int[] planets;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        ScenesScrObj.OnSceneChange += GetSceneValues;
    }

    private void OnDisable()
    {
        ScenesScrObj.OnSceneChange -= GetSceneValues;
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
        gravity = sceneSo.Gravity;
        planets = sceneSo.Planets;
    }

    #endregion
}