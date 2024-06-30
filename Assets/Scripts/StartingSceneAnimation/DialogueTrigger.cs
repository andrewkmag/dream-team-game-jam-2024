using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private Dialogue[] dialogues;

    #region Events

    public static event System.Action<Dialogue[]> OnDialoguesTrigger;

    #endregion
    private void Start()
    {
        OnDialoguesTrigger?.Invoke(dialogues);
    }
}
