using UnityEngine;

public class AudioClipLibrary : GenericLibrary<AudioClip>
{
    [SerializeField] private AudioClipLibScriptableObject audioClipLib;
    
    protected override void getGenericScriptableObject()
    {
        genericLib = audioClipLib;
    }
}