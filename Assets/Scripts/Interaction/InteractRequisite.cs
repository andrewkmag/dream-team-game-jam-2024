using UnityEngine;

public class InteractRequisite : IsInteractable
{
    #region Fields

    [SerializeField] private string contextText = $"Press e to Interact with main objective";
    [SerializeField] private bool requisiteInteracted;
    private static InteractRequisite instance;
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
        if(requisiteInteracted) return;
        requisiteInteracted = true;
        base.DoInteraction();
        GameManager.Instance.RequisiteAchived(true);
        EndInteract();
        Destroy(this);
    }

    #endregion
}