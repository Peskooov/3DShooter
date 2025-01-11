using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoiseAudioSource : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip clip
    {
        get { return audioSource.clip; }
        set { audioSource.clip = value; }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.Play();

        foreach (var dest in Destructible.AllDestructable)
        {
            if (dest is ISoundListener)
            {
                (dest as ISoundListener).Heard(Vector3.Distance(transform.position, dest.transform.position));
            }
        }
    }
}