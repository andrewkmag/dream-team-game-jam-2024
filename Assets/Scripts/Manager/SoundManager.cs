using eXplorerJam.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    public AudioSource source;
    public AudioClip jump, dash, walk, collect;


    public void Update()
    {
        jumpFX();
        dashFX();
    }
    public void jumpFX()
    {
        if(inputReader.jump == true)
        {
            source.clip = jump;
            source.Play();
        }
    }

    public void dashFX()
    {
        if (inputReader.dash == true)
        {
            source.clip = dash;
            source.Play();
        }

    }

    public void walkFX()
    {
       

        

    }

    public void collectFX()
    {
        if (inputReader.interact == true)
        {
            source.clip = collect;
            source.Play();
        }

    }
}
