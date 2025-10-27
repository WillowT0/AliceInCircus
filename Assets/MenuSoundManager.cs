using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class MenuSoundManager : MonoBehaviour
{
    
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}