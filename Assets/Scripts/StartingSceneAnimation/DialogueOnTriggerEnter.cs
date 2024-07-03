using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DialogueOnTriggerEnter : MonoBehaviour
{
    #region Fields

    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private bool _repeatDialogue;

    #endregion

    #region Events

    public static event System.Action<Dialogue[]> OnDialoguesTrigger;

    #endregion

    #region UnityMethods

    private void OnTriggerEnter(Collider other)
    {
        OnDialoguesTrigger?.Invoke(dialogues);
        if (!_repeatDialogue)
        {
            Destroy(this);
        }
    }

    #endregion
}