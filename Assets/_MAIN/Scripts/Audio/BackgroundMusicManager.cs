using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClipLibrary))]
public class BackgroundMusicManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioClipLibrary audioClipLib;
    [SerializeField] private AudioSource audioSource;

    #endregion

    #region UnityMethods

    private void Reset()
    {
        audioClipLib = gameObject.GetOrAdd<AudioClipLibrary>();
        audioSource = gameObject.GetOrAdd<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = audioClipLib.GetGeneric("Default");
        audioSource.loop = true;
        PlayBgMusic();
    }

    #endregion

    #region Methods

    private void PlayBgMusic()
    {
        audioSource.Play();
    }

    private void PauseBgMusic()
    {
        audioSource.Pause();
    }

    #endregion
}