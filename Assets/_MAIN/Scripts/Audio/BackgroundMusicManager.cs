using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClipLibrary))]
public class BackgroundMusicManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioClipLibrary backgroundMusicLib;
    [SerializeField] private AudioSource audioSource;

    #endregion

    #region Constants

    private const float VOLUME_BG = .1f;
    private const string BG_MUSIC_CAT = "BG"; 
    private const string DEFAULT_MUSIC = "Default";
    private const string ANIMATION_MUSIC = "Animation";
    private const string PLANETS_MUSIC = "Planets";

    #endregion

    #region UnityMethods

    private void Reset()
    {
        backgroundMusicLib = gameObject.GetOrAdd<AudioClipLibrary>();
        audioSource = gameObject.GetOrAdd<AudioSource>();
    }

    private void Start()
    {
        audioSource.loop = true;
        audioSource.volume = VOLUME_BG;
        BgMusicPlay(BG_MUSIC_CAT+DEFAULT_MUSIC);
    }

    #endregion

    #region Methods

    private void BgMusicPlay(string musicName)
    {
        audioSource.clip = backgroundMusicLib.GetGeneric(musicName);
        audioSource.Play();
    }

    private void BgMusicStop()
    {
        audioSource.Stop();
    }
    
    private void BgMusicPause()
    {
        audioSource.Pause();
    }

    #endregion
}