using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClipLibrary))]
public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClipLibrary audioClipLib;
    [SerializeField] private AudioSource audioSource;

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

    private void PlayBgMusic()
    {
        audioSource.Play();
    }
    private void PauseBgMusic()
    {
        audioSource.Pause();
    }
}
