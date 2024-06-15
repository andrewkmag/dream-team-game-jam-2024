using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : IsInteractable
{
    #region Constants

    private new const string CONTEXT_TEXT = "Press E to interact";

    #endregion
    
    #region Unity Methods

    protected override void Start()
    {
        base.Start();
        SetContextText(CONTEXT_TEXT);
    }

    #endregion

    #region Methods

    protected override void DoInteraction()
    {
        if (!canInteract) return;
        RestartInteract();
    }

    #endregion
}