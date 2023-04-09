using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    //[SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip tileSwapSound;
    [SerializeField] private AudioClip tileMatchSound;

    //[SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource swapAudioSource;
    [SerializeField] private AudioSource matchAudioSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayTileSwapAudio()
    {
        swapAudioSource.PlayOneShot(tileSwapSound);
    }

    public void PlayTileMatchAudio()
    {
        matchAudioSource.PlayOneShot(tileMatchSound);
    }

    public void PlaySound(string audioClipName)
    {
        switch (audioClipName)
        {
            case "swap":
                swapAudioSource.PlayOneShot(tileSwapSound);
                break;
            case "match":
                matchAudioSource.PlayOneShot(tileMatchSound);
                break;
        }
    }
}
