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

    public void ActivateLoop()
    {
        audioSource.loop = true;
    }
    
    public void DeactivateLoop()
    {
        audioSource.loop = false;
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public static void PlaySoundExternally(string soundName, Transform spawnTransform)
    {
        var sound = new GameObject
        {
            transform =
            {
                position = spawnTransform.position
            }
        };
        var asource =sound.GetOrAdd<AudioSource>();
        asource.clip = AudioClipLibrary.Instance.GetGeneric(soundName);
        asource.Play();
        var clipLenght = asource.clip.length;
        Destroy(sound,clipLenght);
    }

    #endregion
}