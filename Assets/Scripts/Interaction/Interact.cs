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
    [SerializeField] private GameObject contextPrompt;
    [SerializeField] private TextMeshProUGUI contextText;

    #endregion

    #region Fields

    private bool _busyInteracting;
    private bool _iscontextPromptNotNull;
    private bool _promptNotNull;
    private bool _notNull;

    #endregion
    
    #region Constants

    private const float INTERACTION_COOLDOWN = 0.5f;
    private const float INTERACTION_DISTANCE = 5;

    #endregion


    #region Unity Methods

    private void Start()
    {
        _notNull = contextPrompt != null;
        _promptNotNull = contextPrompt != null;
        _iscontextPromptNotNull = contextPrompt != null;
    }

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

        if (Physics.Raycast(ray, out var hit, INTERACTION_DISTANCE))
        {
            if (hit.collider.TryGetComponent(out IsInteractable interactable))
            {
                if (_promptNotNull && !contextPrompt.activeInHierarchy)
                {
                    contextPrompt.SetActive(true);
                    contextText.text = interactable.ContextText;
                }

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
            else if (_iscontextPromptNotNull && contextPrompt.activeInHierarchy)
            {
                contextPrompt.SetActive(false);
            }
        }
        else if (_notNull && contextPrompt.activeInHierarchy)
        {
            contextPrompt.SetActive(false);
        }
    }

    #endregion
}