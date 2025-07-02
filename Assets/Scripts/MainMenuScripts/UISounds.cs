using UnityEngine;

public class UISounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonHoverSound;
    public void PlayButtonHoverSound()
    {
        audioSource.clip = buttonHoverSound;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }
}
