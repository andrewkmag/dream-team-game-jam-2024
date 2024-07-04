using UnityEngine;

public class InteractRequisiteDialogue : IsInteractable
{
    #region Fields

    [SerializeField] private string contextText = $"Press e to Interact with main objective";
    [SerializeField] private bool requisiteInteracted;
    private static InteractRequisiteDialogue instance;
    [SerializeField] private Dialogue[] dialogues;

    #endregion

    #region Events

    public static event System.Action<Dialogue[]> OnDialoguesTrigger;

    #endregion

    #region Constants

    private const int NO_REMAINING = 0;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();
        requisiteInteracted = false;
        SetContextText(contextText);
        GameManager.Instance.RequisiteAchived(false);
    }

    #endregion

    #region Methods

    protected override void DoInteraction()
    {
        if (requisiteInteracted) return;
        OnDialoguesTrigger?.Invoke(dialogues);
        requisiteInteracted = true;
        base.DoInteraction();
        EndInteract();
        GameManager.Instance.RequisiteAchived(true);
        Destroy(gameObject);
    }

    #endregion
}