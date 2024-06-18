using System;
using System.Collections;
using UnityEngine;
using eXplorerJam.Input;
using TMPro;

public class Interact : MonoBehaviour
{
    #region Serialized fields

    [SerializeField] private InputReader inputReader;

    #endregion

    #region Fields

    private bool _busyInteracting;
    private bool _promptNotNull;
    private bool _notNull;
    private bool _isContextualManagerInstaceNotNull;

    #endregion
    
    #region Constants

    private const float INTERACTION_COOLDOWN = 0.5f;
    private const float INTERACTION_DISTANCE = 2.5f;

    #endregion


    #region Unity Methods

    private void Awake()
    {
        _isContextualManagerInstaceNotNull = ContextualUIManager.Instace != null;
        if (!_isContextualManagerInstaceNotNull)
        {
            Debug.LogWarning("Interact needs the contextual ui manager on scene");
        }
    }

    private void Update()
    {
        CheckIfInteractable();

        switch (_busyInteracting)
        {
            case false when inputReader.interact:
                _busyInteracting = true;
                break;
            case true:
                StartCoroutine(InteractionCooldown());
                _busyInteracting = false;
                break;
        }
    }

    #endregion

    #region Methods

    private static IEnumerator InteractionCooldown()
    {
        yield return new WaitForSeconds(INTERACTION_COOLDOWN);
    }

    private void CheckIfInteractable()
    {
        var ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out var hit, INTERACTION_DISTANCE))
        {
            if (hit.collider.TryGetComponent(out IsInteractable interactable))
            {
                interactable.SetContextText(interactable.ContextText);
                if (interactable.RequiresPlayer)
                {
                    if (_busyInteracting == true)
                        interactable.Interact(gameObject.transform);
                }
                else
                {
                    if (_busyInteracting == true)
                        interactable.Interact();
                }
            }
            else
            {
                if (_isContextualManagerInstaceNotNull)
                {
                    ContextualUIManager.Instace.HideContextualText();
                }
            }
        }
        else
        {
            if (_isContextualManagerInstaceNotNull)
            {
                ContextualUIManager.Instace.HideContextualText();
            }
        }
    }

    #endregion
}