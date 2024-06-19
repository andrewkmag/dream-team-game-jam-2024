using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class FirstCheckpoint : MonoBehaviour
{
    #region Fields

    private static FirstCheckpoint Instance { get; set; }

    #endregion

    #region UnityMethods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckpointPosition = transform.position + Vector3.up;
        }
    }

    #endregion
}