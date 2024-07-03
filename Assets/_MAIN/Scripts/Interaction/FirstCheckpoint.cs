using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class FirstCheckpoint : MonoBehaviour
{
    #region Fields

    public static FirstCheckpoint Instance { get; set; }

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
        GameManager.Instance.CheckpointPosition = transform.position + Vector3.up;
    }


    public Vector3 GetPosition()
    {
        return transform.position + Vector3.up;
    }

    #endregion
}