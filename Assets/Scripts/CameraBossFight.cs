using UnityEngine;
using Unity.Cinemachine;

public class CameraRiseTrigger : MonoBehaviour
{
    [Header("References")]
    public CinemachineCamera vCam;   // Virtual camera
    public Transform player;
    public Transform triggerPoint;   // Y position where rising starts

    [Header("Settings")]
    public float upwardSpeed = 1f;
    public float maxRiseDistance = 50f;

    private CinemachineCameraOffset cameraOffset;
    private bool rising = false;
    private float initialYOffset;

    [Header("Death Settings")]
    public float deathOffset = 1f; // Grace distance below camera

    private PlayerHealth playerHealth;

    void Start()
    {
        if (vCam == null)
            vCam = GetComponent<CinemachineCamera>();

        cameraOffset = vCam.GetComponent<CinemachineCameraOffset>();
        if (cameraOffset == null)
            cameraOffset = vCam.gameObject.AddComponent<CinemachineCameraOffset>();

        initialYOffset = cameraOffset.Offset.y;

        // Cache PlayerHealth reference
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null || cameraOffset == null || playerHealth == null)
            return;

        // Start camera rising when player crosses trigger point
        if (!rising && player.position.y >= triggerPoint.position.y)
            rising = true;

        // Move camera upward
        if (rising)
        {
            Vector3 offset = cameraOffset.Offset;
            float newY = offset.y + upwardSpeed * Time.deltaTime;
            if (newY > initialYOffset + maxRiseDistance)
                newY = initialYOffset + maxRiseDistance;

            offset.y = newY;
            cameraOffset.Offset = offset;
        }

        // Check if player fell below camera
        float cameraBottomY = vCam.transform.position.y + cameraOffset.Offset.y - Camera.main.orthographicSize;
        if (player.position.y < cameraBottomY - deathOffset)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        if (playerHealth != null)
        {
            // Deal enough damage to kill the player immediately
            playerHealth.TakeDamage(playerHealth.currentHealth);
        }
    }
}
