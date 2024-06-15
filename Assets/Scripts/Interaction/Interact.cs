using System;
using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    #region Fields

    public Transform playerCamRoot;
    public GameObject contextPrompt;
    public TextMeshProUGUI contextText;
    private bool _isInteracting;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (contextText == null)
        {
            contextText = contextPrompt.GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        CheckIfInteractable();

        // if(isInteracting == false)
        //     isInteracting = true;
        // else if (isInteracting == true)
        //     isInteracting = false;
    }

    #endregion

    #region Methods

    private void CheckIfInteractable()
    {
        var ray = new Ray(playerCamRoot.position, playerCamRoot.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            if (hit.collider.TryGetComponent(out IsInteractable interactable))
            {
                if (contextPrompt != null && !contextPrompt.activeInHierarchy)
                {
                    contextPrompt.SetActive(true);
                    contextText.text = interactable.ContextText;
                }
                if (interactable.RequiresPlayer)
                {
                    if(_isInteracting == true)
                        interactable.Interact(gameObject.transform);
                }
                else
                {
                    if(_isInteracting == true)
                        interactable.Interact();
                }
            }
            else if(contextPrompt != null && contextPrompt.activeInHierarchy)
            {
                contextPrompt.SetActive(false);
            }
        }
        else if (contextPrompt != null && contextPrompt.activeInHierarchy)
        {
            contextPrompt.SetActive(false);
        }
    }

    private void CheckIfInteracting()
    {
        var ray = new Ray(playerCamRoot.position, playerCamRoot.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            if (hit.collider.TryGetComponent(out IsInteractable interactable))
            {
                interactable.Interact(gameObject.transform);
            }
        }
        _isInteracting = false;
    }

    #endregion

}

