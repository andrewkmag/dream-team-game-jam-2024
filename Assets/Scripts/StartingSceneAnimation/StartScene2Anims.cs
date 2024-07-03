using System.Collections;
using UnityEngine;

public class StartScene2Anims : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator animator;
    [SerializeField] private Animator shakingShip;
    [SerializeField] private Animator enemyCharge;
    [SerializeField] private GameObject speechBubbles1;
    [SerializeField] private GameObject speechBubbles2;
    [SerializeField] private GameObject speechBubbles3;

    [SerializeField] private ImageScroller bgImageScroller;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    private static readonly int IsBeingBoarded = Animator.StringToHash("isBeingBoarded");
    private static readonly int IsCharging = Animator.StringToHash("isCharging");

    #endregion

    #region Constants

    private const float WAIT_ANIMATION_TIME = 12f;
    private const int FIRST_DIALOGUE_EVENT = 1;
    private const int SECOND_DIALOGUE_EVENT = 2;
    private const int THIRD_DIALOGUE_EVENT = 4;

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
        animator.SetBool(IsOpen, true);
    }

    private void EndDialogue()
    {
        speechBubbles1.SetActive(false);
        speechBubbles2.SetActive(false);
        speechBubbles3.SetActive(false);
        animator.SetBool(IsOpen, false);
        shakingShip.SetBool(IsBeingBoarded, true);
        enemyCharge.SetBool(IsCharging, true);
        bgImageScroller.enabled = false;
        StartCoroutine(WaitForAnims());
    }

    private void DialogueShow(int dialogueN)
    {
        switch (dialogueN)
        {
            case FIRST_DIALOGUE_EVENT:
                speechBubbles1.SetActive(true);
                speechBubbles2.SetActive(false);
                speechBubbles3.SetActive(false);
                break;
            case SECOND_DIALOGUE_EVENT:
                speechBubbles1.SetActive(false);
                speechBubbles2.SetActive(true);
                speechBubbles3.SetActive(false);
                break;
            case THIRD_DIALOGUE_EVENT:
                speechBubbles1.SetActive(false);
                speechBubbles2.SetActive(false);
                speechBubbles3.SetActive(true);
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