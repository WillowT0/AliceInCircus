using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.UI;     

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Slider musicSlider;

    void Start()
    {
        float currentVolume;
        if (mainMixer.GetFloat("MusicVolume", out currentVolume))
        {
            musicSlider.value = Mathf.Pow(10, currentVolume / 20);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        float volumeInDecibels = Mathf.Log10(sliderValue) * 20;
        
        mainMixer.SetFloat("MusicVolume", volumeInDecibels);
    }
}