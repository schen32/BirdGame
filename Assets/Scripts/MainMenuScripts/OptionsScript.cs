using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScript : MonoBehaviour
{
    [Header("UI Components")]
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    [Header("Audio")]
    public AudioMixer audioMixer; // Reference to your Audio Mixer

    private void Start()
    {
        // Load saved values or defaults
        float savedVolume = PlayerPrefs.GetFloat("volume", 1.0f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        bool isFullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        SetFullscreen(isFullscreen);
    }

    public void SetVolume(float input)
    {
        float volume = volumeSlider.value;
        // Audio mixer expects volume in decibels
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool input)
    {
        bool isFullscreen = fullscreenToggle.isOn;

        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }
}
