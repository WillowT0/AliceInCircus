using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        float currentMusicVolume;
        if (mainMixer.GetFloat("MusicVolume", out currentMusicVolume))
        {
            musicSlider.value = Mathf.Pow(10, currentMusicVolume / 20);
        }

        float currentSfxVolume;
        if (mainMixer.GetFloat("SFXVolume", out currentSfxVolume))
        {
            sfxSlider.value = Mathf.Pow(10, currentSfxVolume / 20);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        float safeValue = Mathf.Max(sliderValue, 0.0001f);
        float volumeInDecibels = Mathf.Log10(safeValue) * 20;
        mainMixer.SetFloat("MusicVolume", volumeInDecibels);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float safeValue = Mathf.Max(sliderValue, 0.0001f);
        float volumeInDecibels = Mathf.Log10(safeValue) * 20;
        mainMixer.SetFloat("SFXVolume", volumeInDecibels);
    }
}