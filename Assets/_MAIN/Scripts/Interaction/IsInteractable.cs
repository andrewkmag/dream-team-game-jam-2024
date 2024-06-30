using System.Collections;
using UnityEngine;

public abstract class IsInteractable : MonoBehaviour
{
    #region Fields

    protected bool canInteract;

    #endregion

    #region Properties

    public bool CanInteract => canInteract;
    public bool RequiresPlayer { get; private set; }

    public string ContextText { get; private set; }

    #endregion

    #region Unity Methods

    protected virtual void Start()
    {
        AbleInteract();
    }

    #endregion

    #region Methods

    private IEnumerator ActivateInteract()
    {
        yield return new WaitForSeconds(1);
        canInteract = true;
    }
    
    public void SetContextText(string context)
    {
        ContextText = context;
        ContextualUIManager.Instace.ShowContextualText(ContextText);
    }
    
    protected void RequiredPlayer()
    {
        RequiresPlayer = true;
    }

    protected void EndInteract()
    {
        canInteract = false;
        ContextualUIManager.Instace.HideContextualText();
    }
    
    protected void RestartInteract()
    {
        StartCoroutine(ActivateInteract());
    }

    private void AbleInteract()
    {
        canInteract = true;
    }

    protected virtual void DoInteraction()
    {
        if (!canInteract) return;
        canInteract = false;
    }
    protected virtual void DoInteraction(Transform interactedTarget)
    {
        if (!canInteract) return;
        canInteract = false;
    }

    public virtual void Interact()
    {
        DoInteraction();
    }

    public virtual void Interact(Transform interactedTarget)
    {
        DoInteraction(interactedTarget);
    }

    #endregion
}