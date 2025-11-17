using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Komponenty")]
    public AudioSource audioSource;

    [Header("Kroki")]
    public AudioClip[] stepClips;
    
    [Tooltip("Co ile sekund ma być dźwięk podczas chodzenia")]
    public float walkInterval = 0.5f;
    
    [Tooltip("Co ile sekund ma być dźwięk podczas sprintu")]
    public float runInterval = 0.3f;

    [Range(0.8f, 1.2f)] public float walkPitch = 1.0f;
    [Range(1.1f, 1.6f)] public float sprintPitch = 1.4f;

    [Header("Skok")]
    public AudioClip jumpClip;
    
    [Range(0f, 1f)] 
    public float jumpVolume = 0.5f;

    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isGrounded;

    private float stepTimer;
    private int lastPlayedIndex = -1;

    void Update()
    {
        if (!isMoving || !isGrounded)
        {
            stepTimer = 0f; 
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            PlayStep();
            stepTimer = isSprinting ? runInterval : walkInterval;
        }
    }

    public void PlayStep()
    {
        if (stepClips.Length == 0) return;

        int newIndex;
        if (stepClips.Length == 1) newIndex = 0;
        else
        {
            do { newIndex = Random.Range(0, stepClips.Length); } 
            while (newIndex == lastPlayedIndex);
        }
        lastPlayedIndex = newIndex;

        float targetPitch = isSprinting ? sprintPitch : walkPitch;
        audioSource.pitch = Random.Range(targetPitch - 0.05f, targetPitch + 0.05f);
        
        audioSource.PlayOneShot(stepClips[newIndex]);
    }

    public void PlayJump()
    {
        if (jumpClip != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(jumpClip, jumpVolume);
        }
    }
}