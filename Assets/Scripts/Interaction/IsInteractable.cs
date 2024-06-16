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

    protected void SetContextText(string context)
    {
        ContextText = context;
    }
    
    protected void RequiredPlayer()
    {
        RequiresPlayer = true;
    }

    protected void EndInteract()
    {
        canInteract = false;
        ContextText = "";
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
        //RestartInteract();//add after override if interaction needs to restart
        //EndInteract(); //override and add instead of StartCoroutine to avoid interacting again
    }

    //interactedTarget sends the information of the player to the interacted object
    protected virtual void DoInteraction(Transform interactedTarget)
    {
        if (!canInteract) return;
        canInteract = false;
        //RestartInteract();//add after override if interaction needs to restart
        //EndInteract(); //override and add instead of StartCoroutine to avoid interacting again
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