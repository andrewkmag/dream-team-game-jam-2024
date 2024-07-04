using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Fields

    [Header("References")] [SerializeField]
    private Image dialogueBox;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image nameBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button contextButton;
    [SerializeField] private TextMeshProUGUI contextBttnText;
    private SoundManager _soundManager;
    private int whosTalking;

    [SerializeField] private Sprite[] variableNameSprites;

    private Queue<Dialogue> _dialogueQueue;

    public float typingSpeed = DEFAULT_TYPING_SPEED;
    private int _actualDialogue;
    private bool _inWorldDialogue;

    #endregion

    #region Constants

    private const int STARTING_DIALOGUE = 0;
    private const int NO_SENTENCTES = 0;
    
    private const string JAM_NAME = "Jam";
    private const string JELLY_NAME = "Jelly";
    private const string RAZZ_NAME = "Razz";
    private const string BARRY_NAME = "Barry";
    private const string ENEMY_NAME = "The Mochi";
    private const string STICKY_NAME = "Sticky";
    private const string CONSOLE_NAME = "Console";
    private const string EVERYONE_NAME = "Everyone";
    
    private const int DIALOGUEBOX_STICKY = 1;
    private const int DIALOGUEBOX_NORMAL = 0;
    private const float DEFAULT_TYPING_SPEED = 0.1f;
    private const float TIME_STOP = 0;
    private const float TIME_CONTINUE = 1;
    
    private const int CHARACTER_TALK = 0;
    private const int ENEMY_TALK = 1;
    private const int CONSOLE_TALK = 2;
    private const int STICKY_TALK = 3;
    
    private const string DIALOGUE_SOUND_CAT = "Dialogue";
    private const string CHAR_SOUND_KEY = "Character";
    private const string ENEMY_SOUND_KEY = "Enemy";
    private const string CONSOLE_SOUND_KEY = "Console";

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
        DialogueOnStart.OnDialoguesTrigger += StartDialogue;
        DialogueOnTriggerEnter.OnDialoguesTrigger += StartDialogueWorld;
    }


    private void OnDisable()
    {
        DialogueOnStart.OnDialoguesTrigger -= StartDialogue;
        DialogueOnTriggerEnter.OnDialoguesTrigger -= StartDialogueWorld;
    }

    private void Awake()
    {
        _soundManager = GetComponent<SoundManager>();
        if (_soundManager == null)
            Debug.LogWarning($"Audio error, {gameObject.name} is mising a soundManager");
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

    private void StartDialogueWorld(Dialogue[] dialogues)
    {
        _inWorldDialogue = true;
        EnterDialogueWorldMode();
        StartDialogue(dialogues);
    }

    private void StartDialogue(Dialogue[] dialogues)
    {
        ShowDialogueBox();
        OnStartDialogue?.Invoke();
        _dialogueQueue = new Queue<Dialogue>();
        _actualDialogue = STARTING_DIALOGUE;
        foreach (var dialogue in dialogues)
        {
            _dialogueQueue.Enqueue(dialogue);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        _actualDialogue++;
        OnDialogueShow?.Invoke(_actualDialogue);
        if (_dialogueQueue.Count <= NO_SENTENCTES)
        {
            EndDialogue();
            return;
        }

        var dialogue = _dialogueQueue.Dequeue();

        whosTalking = WhoIsTalking(dialogue.name);
        nameBox.sprite = whosTalking == STICKY_TALK
            ? variableNameSprites[DIALOGUEBOX_STICKY]
            : variableNameSprites[DIALOGUEBOX_NORMAL];

        nameText.text = dialogue.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogue.sentence));
    }

    private static int WhoIsTalking(string dialogueName)
    {
        return dialogueName switch
        {
            STICKY_NAME => STICKY_TALK,
            JAM_NAME => CHARACTER_TALK,
            JELLY_NAME => CHARACTER_TALK,
            RAZZ_NAME => CHARACTER_TALK,
            BARRY_NAME => CHARACTER_TALK,
            EVERYONE_NAME => CHARACTER_TALK,
            ENEMY_NAME => ENEMY_TALK,
            CONSOLE_NAME => CONSOLE_TALK,
            _ => CONSOLE_TALK
        };
    }

    private IEnumerator TypeSentence(string sentence)
    {
        switch (whosTalking)
        {
            case STICKY_TALK:
            case CHARACTER_TALK:
                _soundManager.PlaySound(DIALOGUE_SOUND_CAT+CHAR_SOUND_KEY);
                break;
            case ENEMY_TALK:
                _soundManager.PlaySound(DIALOGUE_SOUND_CAT+ENEMY_SOUND_KEY);
                break;
            case CONSOLE_TALK:
                _soundManager.PlaySound(DIALOGUE_SOUND_CAT+CONSOLE_SOUND_KEY);
                break;
            default:
                break;
        }

        dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        _soundManager.StopSound();
    }

    private void EndDialogue()
    {
        if (_inWorldDialogue)
        {
            ExitDialogueWorldMode();
        }

        OnEndDialogue?.Invoke();
    }

    private void EnterDialogueWorldMode()
    {
        ShowDialogueBox();
        Time.timeScale = TIME_STOP;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _inWorldDialogue = true;
    }

    private void ExitDialogueWorldMode()
    {
        HideDialogueBox();
        Time.timeScale = TIME_CONTINUE;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _inWorldDialogue = false;
    }

    #endregion
}