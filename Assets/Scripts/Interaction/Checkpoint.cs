using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Checkpoint : MonoBehaviour
{
    #region Fields

    [SerializeField] private bool isActiveCheckpoint;
    private Animator _anim;
    private static readonly int DeactivateCheckpoint = Animator.StringToHash("Deactivate");
    private static readonly int ActivateCheckpoint = Animator.StringToHash("Activate");

    #endregion

    #region Constants

    private const string PLAYER_TAG = "Player";

    #endregion

    #region Events

    public static event System.Action<Checkpoint> OnCheckpointSave;

    #endregion

    #region UnityMethods

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(PLAYER_TAG)) return;
        OnCheckpointSave?.Invoke(this);
    }

    public void Activate()
    {
        if (isActiveCheckpoint) return;
        isActiveCheckpoint = true;
        SetCheckPointRespawn();
        _anim.SetTrigger(ActivateCheckpoint);
    }

    private void SetCheckPointRespawn()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckpointPosition=transform.position+Vector3.up;            
        }
    }

    public void Deactivate(Checkpoint checkpoint)
    {
        if (!isActiveCheckpoint || checkpoint == this) return;
        isActiveCheckpoint = false;
        _anim.SetTrigger(DeactivateCheckpoint);
    }

    #endregion
}