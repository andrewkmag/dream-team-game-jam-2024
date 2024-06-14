using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SceneChanger : MonoBehaviour
{
    #region Fields

    private static SceneChanger _instance;
    [SerializeField] private Animator animator;
    private static readonly int Start = Animator.StringToHash("Start");
    private static readonly int End = Animator.StringToHash("End");

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        ScenesScrObj.OnChange += ChangeScene;
        ScenesScrObj.OnTransition += TransitionScene;
    }


    private void OnDisable()
    {
        ScenesScrObj.OnChange -= ChangeScene;
        ScenesScrObj.OnTransition += TransitionScene;
    }

    private void Reset()
    {
        GetAnimator();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            GetAnimator();
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Methods

    private static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void TransitionScene(string sceneName, float transitionTime)
    {
        StartCoroutine(Transitioning(sceneName, transitionTime, animator));
    }

    private static IEnumerator Transitioning(string sceneName, float transitionTime, Animator animator)
    {
        animator.SetTrigger(Start);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        animator.SetTrigger(End);
    }

    private void GetAnimator()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    #endregion
}