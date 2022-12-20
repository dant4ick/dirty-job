using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource musicSource, effectSource;

    public AudioSource MusicSource { get { return musicSource; } }

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    public static void PlayWeaponSound(AudioClip clip)
    {
        instance.effectSource.PlayOneShot(clip, 0.5f);
    }

    public static void PlayEnvironmentSound(AudioClip clip)
    {
        instance.effectSource.PlayOneShot(clip, 0.3f);
    }

    public static void PlayCaracterSound(AudioClip clip)
    {
        instance.effectSource.PlayOneShot(clip, 0.3f);
    }

    public static void PlayMicrowaveSound(AudioClip clip)
    {
        instance.effectSource.PlayOneShot(clip, 0.6f);
    }
}
