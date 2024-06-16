using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCollectable : IsInteractable
{
    #region Constants

    private new const string CONTEXT_TEXT = "Press E to collect";

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
        base.DoInteraction();
        EndInteract();
        gameObject.SetActive(false);
    }

    #endregion
}