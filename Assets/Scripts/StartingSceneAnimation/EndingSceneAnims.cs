using System.Collections;
using UnityEngine;

public class EndingSceneAnims : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator dialogueBoxAnimator;
    [SerializeField] private Animator shipAnimator;

    [SerializeField] private ImageScroller bgImageScroller;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    private static readonly int BarrellRoll = Animator.StringToHash("barrellRoll");

    #endregion

    #region Constants

    private const float WAIT_ANIMATION_TIME = 1.5f;
    private const int FIRST_DIALOGUE_EVENT = 10;

    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        DialogueManager.OnEndDialogue += EndDialogue;
        DialogueManager.OnStartDialogue += StartDialogue;
        DialogueManager.OnDialogueShow += DialogueShow;
    }

    private void OnDisable()
    {
        DialogueManager.OnEndDialogue -= EndDialogue;
        DialogueManager.OnStartDialogue -= StartDialogue;
        DialogueManager.OnDialogueShow -= DialogueShow;
    }

    #endregion

    #region Methods

    private void StartDialogue()
    {
        dialogueBoxAnimator.SetBool(IsOpen, true);
    }

    private void EndDialogue()
    {
        dialogueBoxAnimator.SetBool(IsOpen, false);
        bgImageScroller.enabled = false;
        StartCoroutine(WaitForAnims());
    }

    private void DialogueShow(int dialogueN)
    {
        switch (dialogueN)
        {
            case FIRST_DIALOGUE_EVENT:
                shipAnimator.SetTrigger(BarrellRoll);
                break;
            default:
                break;
        }
    }

    private static IEnumerator WaitForAnims()
    {
        yield return new WaitForSeconds(WAIT_ANIMATION_TIME);
        SceneChanger.Instance.LoadGameManagerNextScene();
    }

    #endregion
}