using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioSource audioSource;

    #endregion

    #region Constants

    private const float VOLUME_MAX = 1f;

    #endregion

    #region UnityMethods

    private void Reset()
    {
        audioSource = gameObject.GetOrAdd<AudioSource>();
    }

    private void Awake()
    {
        audioSource = gameObject.GetOrAdd<AudioSource>();
    }

    private void Start()
    {
        audioSource.loop = false;
        audioSource.volume = VOLUME_MAX;
    }

    #endregion

    #region Methods

    public void PlaySound(string soundName)
    {
        audioSource.clip = AudioClipLibrary.Instance.GetGeneric(soundName);
        audioSource.Play();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PlaySoundExternally(string soundName, Transform spawnTransform)
    {
        var audioSourceExt = Instantiate(audioSource, spawnTransform.position, Quaternion.identity);
        audioSource.clip = AudioClipLibrary.Instance.GetGeneric(soundName);
        audioSource.Play();
        var clipLenght = audioSource.clip.length;
        Destroy(audioSource.gameObject,clipLenght);
    }

    #endregion
}