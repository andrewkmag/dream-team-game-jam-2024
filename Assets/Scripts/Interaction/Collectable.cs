using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using explorerJam.Managers;

public class Collectable : IsInteractable
{
    #region Constants

    private const string CONTEXT_TEXT = "Press E to collect";

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

        // Trigger the event when a collectable is collected
        if (CollectableManager.Instance != null)
        {
            CollectableManager.Instance.CollectableCollected();
        }

        EndInteract();
        Destroy(gameObject);
    }

    #endregion
}
