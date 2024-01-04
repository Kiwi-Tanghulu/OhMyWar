using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public string soundName;
    public bool onShot;

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    public void Play()
    {
        AudioManager.Instance.PlayAudio(soundName, aud, onShot);
    }
}
