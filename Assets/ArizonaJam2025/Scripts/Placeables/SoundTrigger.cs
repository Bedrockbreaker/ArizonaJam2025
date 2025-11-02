using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundTrigger : Trigger
{
    public AudioSource audioSource;
    public AudioClip soundToPlay;
    public bool isLooping;

    public void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        audioSource.PlayOneShot(soundToPlay);
    }

    public void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

#if UNITY_EDITOR
    public void Reset()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (isLooping) audioSource.loop = true;
    }
#endif
}
