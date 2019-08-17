using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;

    public AudioClip popClip;
    public AudioClip runClip;
    public AudioClip snackClip;
    public AudioClip beverageClip;
    public AudioClip beggingClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _clip, float _volume = 1f)
    {
        audioSource.PlayOneShot(_clip, _volume);
    }
}
