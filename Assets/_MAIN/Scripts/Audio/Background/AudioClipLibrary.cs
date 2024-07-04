using UnityEngine;

public class AudioClipLibrary : GenericLibrary<AudioClip>
{
    #region Fields

    [SerializeField] private AudioClipLibScriptableObject audioClipLib;

    #endregion

    #region Methods

    protected override void getGenericScriptableObject()
    {
        genericLib = audioClipLib;
    }

    #endregion
}