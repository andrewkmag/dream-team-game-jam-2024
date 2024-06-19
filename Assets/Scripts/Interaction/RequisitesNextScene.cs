using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequisitesNextScene : IsInteractable
{
    #region Fields

    [SerializeField] private string contextText = $"Press e to use the spaceship";

    #endregion

    #region Constants

    private const int NO_REMAINING = 0;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (contextText == "")
        {
            contextText = $"Press e to use the spaceship";
        }
    }

    protected override void Start()
    {
        base.Start();
        SetContextText(contextText);
    }

    #endregion

    #region Methods

    protected override void DoInteraction()
    {
        base.DoInteraction();
        GameManager.Instance.SpaceshipUse();
        EndInteract();
    }

    #endregion
}