using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private Image dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image nameBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button contextButton;
    [SerializeField] private TextMeshProUGUI contextBttnText;

    [SerializeField] private Sprite[] variableNameSprites;

    private Queue<Dialogue> _dialogueQueue;

    public float typingSpeed = DEFAULT_TYPING_SPEED;
    private int actualDialogue;

    #endregion
    
    #region Constants

    private const int STARTING_DIALOGUE = 0;
    private const int NO_SENTENCTES = 0;
    private const string STICKY_NAME = "Sticky";
    private const int DIALOGUEBOX_STICKY = 1;
    private const int DIALOGUEBOX_NORMAL = 0;
    private const float DEFAULT_TYPING_SPEED = 0.1f;

    #endregion

    #region Events

    public delegate void Animation();
    public delegate void AnimationEvent(int dialogueN);

    public static event Animation OnEndDialogue;

    public static event Animation OnStartDialogue;

    public static event AnimationEvent OnDialogueShow;

    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        DialogueTrigger.OnDialoguesTrigger += StartDialogue;
    }


    private void OnDisable()
    {
        DialogueTrigger.OnDialoguesTrigger -= StartDialogue;
    }

    private void Awake()
    {
        HideDialogueBox();
    }

    #endregion

    #region Methods

    private void HideDialogueBox()
    {
        dialogueBox.enabled = false;
        dialogueText.enabled = false;
        nameBox.enabled = false;
        nameText.enabled = false;
        contextButton.enabled = false;
        contextBttnText.enabled = false;
    }

    private void ShowDialogueBox()
    {
        dialogueBox.enabled = true;
        dialogueText.enabled = true;
        nameBox.enabled = true;
        nameText.enabled = true;
        contextButton.enabled = true;
        contextBttnText.enabled = true;
    }

    private void StartDialogue(Dialogue[] dialogues)
    {
        ShowDialogueBox();
        OnStartDialogue?.Invoke();
        _dialogueQueue = new Queue<Dialogue>();
        actualDialogue = STARTING_DIALOGUE;
        foreach (var dialogue in dialogues)
        {
            _dialogueQueue.Enqueue(dialogue);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        actualDialogue++;
        OnDialogueShow?.Invoke(actualDialogue);
        if (_dialogueQueue.Count <= NO_SENTENCTES)
        {
            EndDialogue();
            return;
        }

        var dialogue = _dialogueQueue.Dequeue();

        nameBox.sprite = dialogue.name.ToUpper().Equals(STICKY_NAME.ToUpper())
            ? variableNameSprites[DIALOGUEBOX_STICKY]
            : variableNameSprites[DIALOGUEBOX_NORMAL];

        nameText.text = dialogue.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogue.sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private static void EndDialogue()
    {
        OnEndDialogue?.Invoke();
    }

    #endregion
}