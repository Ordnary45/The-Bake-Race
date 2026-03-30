using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class playMusic : MonoBehaviour
{
    public AudioClip musicClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = musicClip;
        audioSource.loop = true;            // loop infinitely
        audioSource.playOnAwake = true;     // auto-play
        audioSource.spatialBlend = 0f;      // 2D sound
        audioSource.volume = 0.3f;          // adjust as needed

        audioSource.Play();
    }
}