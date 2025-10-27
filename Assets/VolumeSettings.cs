using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    
    public AudioMixer mainMixer;

    
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (musicSlider != null)
        {
            float musicVol;
            mainMixer.GetFloat("MusicVolume", out musicVol);
            
            musicSlider.SetValueWithoutNotify(DecibelToLinear(musicVol));
        }

        if (sfxSlider != null)
        {
            float sfxVol;
            mainMixer.GetFloat("SFXVolume", out sfxVol);
            
            sfxSlider.SetValueWithoutNotify(DecibelToLinear(sfxVol));
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        mainMixer.SetFloat("MusicVolume", LinearToDecibel(sliderValue));
    }


    public void SetSFXVolume(float sliderValue)
    {
        mainMixer.SetFloat("SFXVolume", LinearToDecibel(sliderValue));
    }

   
    private float LinearToDecibel(float linear)
    {
     
        if (linear <= 0.0001f)
            return -80.0f;
        
        return Mathf.Log10(linear) * 20.0f;
    }

    
    private float DecibelToLinear(float db)
    {
        if (db <= -80.0f)
            return 0.0001f;
            
        return Mathf.Pow(10.0f, db / 20.0f);
    }
}