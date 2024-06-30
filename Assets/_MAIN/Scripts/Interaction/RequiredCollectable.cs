using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredCollectable : IsInteractable
{
    #region Fields

    [SerializeField] private string contextText = $"Press e to collect required item";
    private static int ReqCollectablesQty = 0;

    #endregion

    #region Constants

    private const int NO_REMAINING = 0;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (contextText == "")
        {
            contextText = $"Press e to collect required item";
        }

        ReqCollectablesQty = NO_REMAINING;
        
    }

    protected override void Start()
    {
        ReqCollectablesQty++;
        base.Start();
        SetContextText(contextText);
        GameManager.Instance.CollectedItem(ReqCollectablesQty);
    }

    #endregion

    #region Methods

    protected override void DoInteraction()
    {
        base.DoInteraction();
        ReqCollectablesQty--;
        GameManager.Instance.CollectedItem(ReqCollectablesQty);
        EndInteract();
        Destroy(gameObject);
    }

    #endregion
}