using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClipLibrary))]
public class BackgroundMusicManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioClipLibrary backgroundMusicLib;
    [SerializeField] private AudioSource audioSource;
    private string actualMusic;

    #endregion

    #region Constants

    private const float VOLUME_BG = .1f;

    private const string BG_MUSIC_CATEGORY = "BG";
    private const string DEFAULT_MUSIC = "Default";
    private const string ANIMATION_MUSIC = "Animation";
    private const string PLANETS_MUSIC = "Planets";

    #endregion

    #region UnityMethods

    private void OnEnable()
    {
        ScenesScrObj.OnSceneChange += SceneChangeBgMusic;
    }

    private void OnDisable()
    {
        ScenesScrObj.OnSceneChange -= SceneChangeBgMusic;
    }

    private void Reset()
    {
        backgroundMusicLib = gameObject.GetOrAdd<AudioClipLibrary>();
        audioSource = gameObject.GetOrAdd<AudioSource>();
    }

    private void Start()
    {
        audioSource.loop = true;
        audioSource.volume = VOLUME_BG;
        BgMusicPlay(BG_MUSIC_CATEGORY + DEFAULT_MUSIC);
        actualMusic = BG_MUSIC_CATEGORY + DEFAULT_MUSIC;
    }

    #endregion

    #region Methods

    private void SceneChangeBgMusic(ScenesScrObj sceneSo)
    {
        var songToplay = sceneSo.Song switch
        {
            PLANETS_MUSIC => BG_MUSIC_CATEGORY + PLANETS_MUSIC,
            ANIMATION_MUSIC => BG_MUSIC_CATEGORY + ANIMATION_MUSIC,
            _ => BG_MUSIC_CATEGORY + DEFAULT_MUSIC
        };

        if (!actualMusic.Equals(songToplay))
        {
            actualMusic = songToplay;
            BgMusicPlay(songToplay);
        }
        else
        {
            return;
        }
    }

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