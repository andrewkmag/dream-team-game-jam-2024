using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger _instance;
    [SerializeField] private Animator animator;
    private static readonly int Start = Animator.StringToHash("Start");
    private static readonly int End = Animator.StringToHash("End");

    #region UnityMethods
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


    private void Reset()
    {
        GetAnimator();
    }

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

    #endregion

    #region Methods

    private static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    private void TransitionScene(string sceneName, float transitionTime)
    {
        StartCoroutine(Transitioning(sceneName,transitionTime,animator));
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