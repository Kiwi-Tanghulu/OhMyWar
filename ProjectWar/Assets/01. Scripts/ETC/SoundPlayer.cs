using System;
using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public string[] soundNames;
    public string soundName;
    public bool onShot;
    public float delay;

    [SerializeField] AudioSource aud;

    private void Awake()
    {
        if(aud == null)
            aud = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if(string.IsNullOrEmpty(soundName))
            soundName = soundNames.PickRandom();

        if(delay == 0f)
            AudioManager.Instance.PlayAudio(soundName, aud, onShot);
        else
            StartCoroutine(DelayCoroutine(delay, () => AudioManager.Instance.PlayAudio(soundName, aud, onShot)));
    }

    private IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
