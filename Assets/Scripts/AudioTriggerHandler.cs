using UnityEngine;

public class AudioTriggerHandler : MonoBehaviour
{
    public DynamicCartAudio dynamicAudio;

    [Header("Источники")]
    public AudioSource liftSource;
    public AudioSource sfxSource;

    [Header("Клипы")]
    public AudioClip brakeClip;
    public AudioClip birdClip;
    public AudioClip boostClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Haptic_Lift"))
        {
            if (!liftSource.isPlaying) liftSource.Play();
            if (dynamicAudio != null) dynamicAudio.isOnLift = true;
        }

        else if (other.CompareTag("Haptic_Bird"))
        {
            if (sfxSource != null && birdClip != null)
                sfxSource.PlayOneShot(birdClip, 2.0f);
        }
        else if (other.CompareTag("Haptic_Brake"))
        {
            if (sfxSource != null && brakeClip != null)
                sfxSource.PlayOneShot(brakeClip, 3.0f);
        }
        else if (other.CompareTag("Haptic_Boost"))
        {
            if (sfxSource != null && boostClip != null)
                sfxSource.PlayOneShot(boostClip, 2.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Haptic_Lift"))
        {
            liftSource.Stop();
            if (dynamicAudio != null) dynamicAudio.isOnLift = false;
        }
    }
}