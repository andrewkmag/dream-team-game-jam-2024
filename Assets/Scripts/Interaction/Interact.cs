using System;
using System.Collections;
using UnityEngine;
using eXplorerJam.Input;
using TMPro;

public class Interact : MonoBehaviour
{
    #region Serialized fields

    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform playerCamRoot;

    #endregion

    #region Fields

    private bool _busyInteracting;
    private bool _promptNotNull;
    private bool _notNull;

    #endregion
    
    #region Constants

    private const float INTERACTION_COOLDOWN = 0.5f;
    private const float INTERACTION_DISTANCE = 2.5f;

    #endregion


    #region Unity Methods

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
        var ray = new Ray(playerCamRoot.position, playerCamRoot.forward);
        Debug.Log("Oscar On interaction distance");
        if (Physics.Raycast(ray, out var hit, INTERACTION_DISTANCE))
        {
            Debug.Log("Oscar On interaction distance");
            if (hit.collider.TryGetComponent(out IsInteractable interactable))
            {
                Debug.Log("Oscar found interactable");
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
                ContextualUIManager.Instace.HideContextualText();                
            }
        }
        else
        {
            ContextualUIManager.Instace.HideContextualText();                
        }
    }

    #endregion
}