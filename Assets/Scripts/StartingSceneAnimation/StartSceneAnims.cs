using System.Collections;
using UnityEngine;

public class StartSceneAnims : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator dialogueBoxAnimator;
    [SerializeField] private Animator shipAnimator;

    [SerializeField] private ImageScroller bgImageScroller;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    private static readonly int StopShip = Animator.StringToHash("StopShip");

    #endregion

    #region Constants

    private const float WAIT_ANIMATION_TIME = 1.5f;

    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        DialogueManager.OnEndDialogue += EndDialogue;
        DialogueManager.OnStartDialogue -= StartDialogue;
    }

    private void OnDisable()
    {
        DialogueManager.OnEndDialogue -= EndDialogue;
        DialogueManager.OnStartDialogue -= StartDialogue;
    }

    #endregion

    private void StartDialogue()
    {
        dialogueBoxAnimator.SetBool(IsOpen, true);
    }

    #region Methods

    private void EndDialogue()
    {
        dialogueBoxAnimator.SetBool(IsOpen, false);
        shipAnimator.SetBool(StopShip, true);
        bgImageScroller.enabled = false;
        StartCoroutine(WaitForAnims());
    }


    private static IEnumerator WaitForAnims()
    {
        yield return new WaitForSeconds(WAIT_ANIMATION_TIME);
        SceneChanger.Instance.LoadGameManagerNextScene();
    }

    #endregion
}