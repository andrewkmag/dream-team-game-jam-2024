using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    #region Properties

    private static Checkpoint CheckpointActive { get; set; }

    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        Checkpoint.OnCheckpointSave += CheckpointUpdate;
    }


    private void OnDisable()
    {
        Checkpoint.OnCheckpointSave -= CheckpointUpdate;
    }

    #endregion

    private static void CheckpointUpdate(Checkpoint checkpoint)
    {
        if (CheckpointActive == null)
        {
            checkpoint.Activate();
        }
        else
        {
            CheckpointActive.Deactivate(checkpoint);
            checkpoint.Activate();
        }

        CheckpointActive = checkpoint;
    }
}