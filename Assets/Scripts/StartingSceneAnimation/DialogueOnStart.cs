using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueOnStart : MonoBehaviour
{
    #region Fields

    [SerializeField] private Dialogue[] dialogues;

    #endregion

    #region Events

    public static event System.Action<Dialogue[]> OnDialoguesTrigger;

    #endregion

    #region UnityMethods

    private void Start()
    {
        OnDialoguesTrigger?.Invoke(dialogues);
    }

    #endregion
}